using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using quoteRedux.Models;
using Microsoft.Extensions.Options;

namespace quoteRedux.Factory
{
    public class QuoteFactory : IFactory<Quote>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public QuoteFactory(IOptions<MySqlOptions> conf)
        {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
        public void Add(Quote item, int userID)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO quotes (text, user_id, created_at, updated_at) VALUES (@Text, {userID}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }
        public IEnumerable<Quote> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = "select * from quotes join users on users.id = quotes.user_id";
                dbConnection.Open();
                
                var myQuotes = dbConnection.Query<Quote, User, Quote>(query, (Quote, user) => { Quote.user = user; return Quote; });
                return myQuotes;
            }
        }
        public Quote FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Quote>("SELECT * FROM quotes WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
        public void DeleteByID(int id)
        {
            using(IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query<Quote>("DELETE FROM quotes WHERE id = @Id", new { Id = id }).FirstOrDefault();
                
            }
        }
        public void Edit(int ID, string Text)
        {
            using(IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query<Quote>($"UPDATE quotes SET text='{Text}' WHERE id={ID}");
            }
        }
    }
}