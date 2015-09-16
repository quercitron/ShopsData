using System;
using System.Collections.Generic;

using DataCollectorCore.DataObjects;

using Npgsql;

using NpgsqlTypes;

namespace PostgreDAL
{
    public class ShopsDataStore : IShopsDataStore
    {
        private readonly string _connectionString;

        private readonly string _dbName;
        private readonly string _user = "shopsuser";
        private readonly string _password = "123123";

        public ShopsDataStore(string dbName)
        {
            if (dbName == null)
            {
                throw new ArgumentNullException("dbName");
            }

            _dbName = dbName;

            _connectionString = string.Format(
                "Server=127.0.0.1;Port=5433;User Id={0};Password={1};Database={2};",
                _user,
                _password,
                _dbName);
        }

        public List<ProductType> GetProductTypes()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select producttypeid, name from producttype", conn);

            var productTypes = new List<ProductType>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var productType = new ProductType();

                    productType.ProductTypeId = dr.GetInt32(0);
                    productType.Name = dr.GetString(1);

                    productTypes.Add(productType);
                }

            }
            finally
            {
                conn.Close();
            }

            return productTypes;
        }

        public void AddProductType(string productType)
        {
            var commandText = string.Format("insert into ProductType (Name) values ('{0}')", productType);
            ExecuteNonQueryCommand(commandText);
        }

        public List<DataSource> GetDataSources()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select datasourceid, name from datasource", conn);

            var dataSources = new List<DataSource>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var dataSource = new DataSource();

                    dataSource.DataSourceId = dr.GetInt32(0);
                    dataSource.Name = dr.GetString(1);

                    dataSources.Add(dataSource);
                }

            }
            finally
            {
                conn.Close();
            }

            return dataSources;
        }

        public void AddDataSource(string dataSourceName)
        {
            var commandText = string.Format("insert into datasource (Name) values ('{0}')", dataSourceName);
            ExecuteNonQueryCommand(commandText);
        }

        public List<Product> GetProducts()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select productid, name, producttypeid from product", conn);

            var products = new List<Product>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var product = new Product();

                    product.ProductId = dr.GetInt32(0);
                    product.Name = dr.GetString(1);
                    product.ProductTypeId = dr.GetInt32(2);

                    products.Add(product);
                }

            }
            finally
            {
                conn.Close();
            }

            return products;
        }

        public List<Product> GetProducts(string productTypeName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "select p.productid, p.name, p.producttypeid " +
                              "from product p join producttype pt on p.producttypeid = pt.producttypeid " +
                              "where pt.name = :producttypename";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("producttypename", NpgsqlDbType.Text, productTypeName);

            var products = new List<Product>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var product = new Product();

                    product.ProductId = dr.GetInt32(0);
                    product.Name = dr.GetString(1);
                    product.ProductTypeId = dr.GetInt32(2);

                    products.Add(product);
                }

            }
            finally
            {
                conn.Close();
            }

            return products;
        }

        public List<Product> GetProducts(int productTypeId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "select productid, name, producttypeid from product " +
                              "where producttypeid = :producttypeid";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, productTypeId);

            var products = new List<Product>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var product = new Product();

                    product.ProductId = dr.GetInt32(0);
                    product.Name = dr.GetString(1);
                    product.ProductTypeId = dr.GetInt32(2);

                    products.Add(product);
                }

            }
            finally
            {
                conn.Close();
            }

            return products;
            
        }

        public void AddProduct(Product product)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = string.Format("insert into product (name, producttypeid) " +
                                            "values (:name, :producttypeid) " +
                                            "returning productid");
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, product.Name);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, product.ProductTypeId);

            try
            {
                product.ProductId = (int) command.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddProductRecord(ProductRecord productRecord)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "insert into productrecord ( sourceproductid,  name,  description,  price,  rating,  amountavailable,  timestamp,  locationid,  externalid,  brand) " +
                              "values                    (:sourceproductid, :name, :description, :price, :rating, :amountavailable, :timestamp, :locationid, :externalid, :brand) " +
                              "returning productrecordid";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("sourceproductid", NpgsqlDbType.Integer, productRecord.SourceProductId);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, productRecord.Name);
            command.Parameters.AddWithValue("description", NpgsqlDbType.Text, productRecord.Description);
            command.Parameters.AddWithValue("price", NpgsqlDbType.Integer, productRecord.Price);
            command.Parameters.AddWithValue("rating", NpgsqlDbType.Real, productRecord.Rating);
            command.Parameters.AddWithValue("amountavailable", NpgsqlDbType.Integer, productRecord.AmountAvailable);
            command.Parameters.AddWithValue("timestamp", NpgsqlDbType.Timestamp, productRecord.Timestamp);
            command.Parameters.AddWithValue("locationid", NpgsqlDbType.Integer, productRecord.LocationId);
            command.Parameters.AddWithValue("externalid", NpgsqlDbType.Text, productRecord.ExternalId);
            command.Parameters.AddWithValue("brand", NpgsqlDbType.Text, productRecord.Brand);

            try
            {
                productRecord.ProductRecordId = (int) command.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddLocation(Location location)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "insert into location ( name) " +
                              "values               (:name)";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, location.Name);

            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Location> GetLocations()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select locationid, name from location", conn);

            var products = new List<Location>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var location = new Location();

                    location.LocationId = dr.GetInt32(0);
                    location.Name = dr.GetString(1);

                    products.Add(location);
                }

            }
            finally
            {
                conn.Close();
            }

            return products;
        }

        public List<SourceProduct> GetSourceProducts(int dataSourceId, int productTypeId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "select sp.sourceproductid, sp.datasourceid, sp.productid, sp.key, sp.name, sp.originalname, sp.brand, sp.timestamp " +
                              "from sourceproduct sp join product p on sp.productid = p.productid " +
                              "where sp.datasourceid = :datasourceid and p.producttypeid = :producttypeid";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("datasourceid", NpgsqlDbType.Integer, dataSourceId);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, productTypeId);

            var sourceProducts = new List<SourceProduct>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var sourceProduct = new SourceProduct();

                    sourceProduct.SourceProductId = dr.GetInt32(0);
                    sourceProduct.DataSourceId = dr.GetInt32(1);
                    sourceProduct.ProductId = dr.GetInt32(2);
                    sourceProduct.Key = dr.GetString(3);
                    sourceProduct.Name = dr.GetString(4);
                    sourceProduct.OriginalName = dr.GetString(5);
                    sourceProduct.Brand = dr.GetString(6);
                    sourceProduct.Timestamp = dr.GetTimeStamp(7);

                    sourceProducts.Add(sourceProduct);
                }

            }
            finally
            {
                conn.Close();
            }

            return sourceProducts;
            
        }

        public void AddSourceProduct(SourceProduct sourceProduct)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "insert into sourceproduct ( datasourceid,  productid,  key,  name,  originalname,  brand,  timestamp) " +
                              "values                    (:datasourceid, :productid, :key, :name, :originalname, :brand, :timestamp) " +
                              "returning sourceproductid";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("datasourceid", NpgsqlDbType.Integer, sourceProduct.DataSourceId);
            command.Parameters.AddWithValue("productid", NpgsqlDbType.Integer, sourceProduct.ProductId);
            command.Parameters.AddWithValue("key", NpgsqlDbType.Text, sourceProduct.Key);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, sourceProduct.Name);
            command.Parameters.AddWithValue("originalname", NpgsqlDbType.Text, sourceProduct.OriginalName);
            command.Parameters.AddWithValue("brand", NpgsqlDbType.Text, sourceProduct.Brand);
            command.Parameters.AddWithValue("timestamp", NpgsqlDbType.Timestamp, sourceProduct.Timestamp);

            try
            {
                sourceProduct.SourceProductId = (int) command.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
        }

        private void ExecuteNonQueryCommand(string commandText)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);

            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
