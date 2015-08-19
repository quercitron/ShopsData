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

            NpgsqlCommand command = new NpgsqlCommand("select (Name) from datasource", conn);

            var dataSources = new List<DataSource>();
            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var dataSource = new DataSource();

                    dataSource.Name = dr.GetString(0);

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

            var commandText = string.Format("insert into product (name, producttypeid) values (:name, :producttypeid)");
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, product.Name);
            command.Parameters.AddWithValue("producttypeid", NpgsqlDbType.Integer, product.ProductTypeId);

            try
            {
                command.ExecuteNonQuery();
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

            var commandText = "insert into productrecord ( sourceproductid,  name,  description,  price,  rating,  amountavailable,  timestamp,  locationid) " +
                              "values                    (:sourceproductid, :name, :description, :price, :rating, :amountavailable, :timestamp, :locationid)";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("sourceproductid", NpgsqlDbType.Integer, productRecord.SourceProductId);
            command.Parameters.AddWithValue("name", NpgsqlDbType.Text, productRecord.Name);
            command.Parameters.AddWithValue("description", NpgsqlDbType.Text, productRecord.Description);
            command.Parameters.AddWithValue("price", NpgsqlDbType.Integer, productRecord.Price);
            command.Parameters.AddWithValue("rating", NpgsqlDbType.Real, productRecord.Rating);
            command.Parameters.AddWithValue("amountavailable", NpgsqlDbType.Integer, productRecord.AmountAvailable);
            command.Parameters.AddWithValue("timestamp", NpgsqlDbType.Timestamp, productRecord.Timestamp);
            command.Parameters.AddWithValue("locationid", NpgsqlDbType.Integer, productRecord.LocationId);

            try
            {
                command.ExecuteNonQuery();
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

            var commandText = "insert into location ( locationid,  name) " +
                              "values               (:locationid, :name)";
            NpgsqlCommand command = new NpgsqlCommand(commandText, conn);
            command.Parameters.AddWithValue("locationid", NpgsqlDbType.Integer, location.LocationId);
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
