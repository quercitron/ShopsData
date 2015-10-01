using System;
using System.Collections.Generic;
using System.Configuration;

using DataCollectorCore.DataObjects;

using Npgsql;

using NpgsqlTypes;

namespace PostgreDAL
{
    public class ShopsDataStore : IShopsDataStore
    {
        private readonly string _connectionString;

        public ShopsDataStore()
            : this(null)
        {

        }

        public ShopsDataStore(string connectionString)
        {
            if (connectionString != null)
            {
                _connectionString = connectionString;
            }
            else
            {
                _connectionString = ConfigurationManager.ConnectionStrings["ShopsData"].ConnectionString;
            }
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

            NpgsqlCommand command = new NpgsqlCommand("select productid, name, class, producttypeid, created from product", conn);

            var products = new List<Product>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var product = new Product();

                    product.ProductId = dr.GetInt32(0);
                    product.Name = dr.GetString(1);
                    product.Class = dr[2] as string;
                    product.ProductTypeId = dr.GetInt32(3);
                    product.Created = dr.GetTimeStamp(4);

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

            var commandText = "select p.productid, p.name, p.producttypeid, p.class, p.created " +
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
                    product.Class = dr[3] as string;
                    product.Created = dr.GetTimeStamp(4);

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

            var commandText = "select productid, name, producttypeid, class, created from product " +
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
                    product.Class = dr[3] as string;
                    product.Created = dr.GetTimeStamp(4);

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

            var commandText = string.Format("insert into product (name, class, producttypeid, created) " +
                                            "values (:name, :class, :producttypeid, :created) " +
                                            "returning productid");
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, product.Name);
            command.Parameters.AddWithValue("class", NpgsqlDbType.Text, product.Class);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, product.ProductTypeId);
            command.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, product.Created);

            try
            {
                product.ProductId = (int) command.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateProduct(Product product)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = string.Format(
                "update product " +
                "set name = :name, class = :class, producttypeid = :producttypeid, created = :created " +
                "where productid = :productid");
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, product.Name);
            command.Parameters.AddWithValue("class", NpgsqlDbType.Text, product.Class);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, product.ProductTypeId);
            command.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, product.Created);
            command.Parameters.AddWithValue("productid", NpgsqlDbType.Integer, product.ProductId);

            try
            {
                command.ExecuteScalar();
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

            var commandText = "select sp.sourceproductid, sp.datasourceid, sp.productid, sp.key, sp.name, sp.originalname, sp.brand, sp.timestamp, sp.class " +
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
                    sourceProduct.Class = dr.GetString(8);

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

            var commandText = "insert into sourceproduct ( datasourceid,  productid,  key,  name,  class,  originalname,  brand,  timestamp) " +
                              "values                    (:datasourceid, :productid, :key, :name, :class, :originalname, :brand, :timestamp) " +
                              "returning sourceproductid";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("datasourceid", NpgsqlDbType.Integer, sourceProduct.DataSourceId);
            command.Parameters.AddWithValue("productid", NpgsqlDbType.Integer, sourceProduct.ProductId);
            command.Parameters.AddWithValue("key", NpgsqlDbType.Text, sourceProduct.Key);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, sourceProduct.Name);
            command.Parameters.AddWithValue("class", NpgsqlDbType.Text, sourceProduct.Class);
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

        public List<ProductData> GetCurrentData(int locationId, int productTypeId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var commandText = "with list as ( " +
                              "select p.productid, p.name, p.class, p.producttypeid, sp.sourceproductid, sp.datasourceid " +
                              "from product p " +
                              "join sourceproduct sp on p.productid = sp.productid " +
                              "where p.producttypeid = :producttypeid) " +
                              "select list.productid, list.name, list.producttypeid, list.datasourceid, pr1.price, pr1.rating, pr1.timestamp, pr1.locationid, list.class " +
                              "from list " +
                              "join productrecord pr1 on list.sourceproductid = pr1.sourceproductid " +
                              "left join productrecord pr2 on(list.sourceproductid = pr2.sourceproductid and pr1.timestamp < pr2.timestamp) " +
                              "where pr2.productrecordid is null and pr1.locationid = :locationid and pr1.timestamp > current_date - 14";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("locationid", NpgsqlDbType.Integer, locationId);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, productTypeId);

            var products = new List<ProductData>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var product = new ProductData();

                    product.ProductId = dr.GetInt32(0);
                    product.Name = dr.GetString(1);
                    product.ProductTypeId = dr.GetInt32(2);
                    product.DataSourceId = dr.GetInt32(3);
                    product.Price = dr.GetInt32(4);
                    product.Rating = dr.GetFloat(5);
                    product.Timestamp = DateTime.SpecifyKind(dr.GetTimeStamp(6), DateTimeKind.Utc);
                    product.LocationId = dr.GetInt32(7);
                    product.Class = dr[8] as string;

                    products.Add(product);
                }

            }
            finally
            {
                conn.Close();
            }

            return products;
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
