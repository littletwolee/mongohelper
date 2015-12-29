using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
namespace pwc_mongo_helper
{
    public partial class MongoHelperFromEntity<T>: IMongoHelper<T>
    {
        #region variable
        IMongoClient mongoClient = null;
        IMongoCollection<T> collection = null;
        IMongoDatabase db;
        string connString = string.Empty;
        string connDB = string.Empty;
        #endregion
        public MongoHelperFromEntity(string collectionname)
        {
            #region check webconfig
            try
            {
                connString = ConfigurationManager.AppSettings["mongodbConnection"];
            }
            catch (Exception)
            {
                throw new Exception("plese add mongodbConnection in webconfig");
            }
            try
            {
                connDB = ConfigurationManager.AppSettings["mongodbDatabase"];
            }
            catch (Exception)
            {
                throw new Exception("plese add mongodbDatabase in webconfig");
            }
            #endregion 
            if (mongoClient == null)
            {
                mongoClient = new MongoClient(connString);
                db = mongoClient.GetDatabase(connDB);
                collection = db.GetCollection<T>(collectionname);
            }
        }
        #region fun
        private async Task<string> _Insert(T entity)
        {

            string flag = null;
            try
            {
                flag = ObjectId.GenerateNewId().ToString();
                entity.GetType().GetProperty("_id").SetValue(entity, flag);
                await collection.InsertOneAsync(entity);
            }
            catch (Exception) 
            {
                
            }
            return flag;
        }
        Task<UpdateResult> _Push(string id, Expression<Func<T, IEnumerable<object>>> filed, object value)
        {
            Task<UpdateResult> flag = null;
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                flag = collection.UpdateOneAsync(filter, MongoDB.Driver.Builders<T>.Update.Push(filed, value));
            }
            catch (Exception){}
            return flag;
        }
        Task<UpdateResult> _Pull<C>(string id, Expression<Func<T, IEnumerable<C>>> filed, Expression<Func<C, bool>> expression)
        {
            Task<UpdateResult> flag = null;
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                flag = collection.UpdateOneAsync(filter, MongoDB.Driver.Builders<T>.Update.PullFilter(filed,expression));
            }
            catch (Exception) { }
            return flag;
        }
        Task<List<T>> _Find(string[] fields, Dictionary<string, int> sortfields, System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            IFindFluent<T, T> iff = null;
            if (expression == null)
            {
                iff = collection.Find<T>(new BsonDocument());
            }
            else
            {
                iff = collection.Find<T>(expression);
            }
            if (fields.Length > 0)
            {
                Dictionary<string,int> dicfields = new Dictionary<string,int>();
                foreach (string item in fields)
                {
                    dicfields.Add(item,1);
                }
                iff = iff.Project<T>(Tools<T>.getDisplayFiles(dicfields));
            }
            if(sortfields != null)
            {
                iff = iff.Sort(Tools<T>.getSortDefinition(sortfields));
            }
            return iff.ToListAsync();
        }
        Task<T> _FindOne(string id,string[] fields)
        {
            Task<T> result = null;
            IFindFluent<T, T> iff = null;
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                iff = collection.Find<T>(filter);
                if (fields!= null && fields.Length > 0)
                {
                    Dictionary<string, int> dicfields = new Dictionary<string, int>();
                    foreach (string item in fields)
                    {
                        dicfields.Add(item, 1);
                    }
                    iff = iff.Project<T>(Tools<T>.getDisplayFiles(dicfields));
                }
                result = iff.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
            }
            return result;

        }
        Task<ReplaceOneResult> _Replace(string id , object data)
        {
            Task<ReplaceOneResult> result = null;
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                data.GetType().GetProperty("_id").SetValue(data, id);
                result = collection.ReplaceOneAsync(filter, (T)data);
            }
            catch (Exception)
            {
            }
            return result;
        }
        Task<UpdateResult> _Update(string id,Dictionary<string,object> updatedic)
        {
            Task<UpdateResult> result = null;
            if (updatedic.Count > 0)
            {
                try
                {
                    FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                    result = collection.UpdateOneAsync(filter, Tools<T>.getUpdateDefinition(updatedic));
                }
                catch (Exception)
                {
                }
                
            }
            return result;
        }
        Task<DeleteResult> _Delete(string id)
        {
            Task<DeleteResult> result = null;
            try
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
                result =collection.DeleteOneAsync(filter);
            }
            catch (Exception)
            {
                
            }
            return result;
        }
        #endregion

        #region interface implementationurns>
        public string Insert(object entity)
        {
            return _Insert((T)entity).GetAwaiter().GetResult();
        }
        public bool Replace(string id, object data)
        {
            bool flag = false;
            Task<ReplaceOneResult> ur = _Replace(id ,data);
            if (ur != null && ur.Result.ModifiedCount > 0)
            {
                flag = true;
            }
            return flag;
        }
        public bool Update(string id, Dictionary<string, object> updatedic)
        {
            bool flag = false;
            Task<UpdateResult> ur = _Update(id, updatedic);
            if (ur != null && ur.Result.ModifiedCount > 0)
            {
                flag = true;
            }
            return flag;
        }
        public bool Delete(string id)
        {
            bool flag = false;
            Task<DeleteResult> ur = _Delete(id);
            if (ur != null && ur.Result.DeletedCount > 0)
            {
                flag = true;
            }
            return flag;
        }
        public bool Push(string id, Expression<Func<T, IEnumerable<object>>> filed, object value)
        {
            bool flag = false;
            Task<UpdateResult> ur = _Push(id, filed, value);
            try
            {
                if (ur != null && ur.Result.ModifiedCount > 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                
            }
            
            return flag;
        }
        public bool Pull<C>(string id, Expression<Func<T, IEnumerable<C>>> filed, Expression<Func<C, bool>> expression)
        {
            bool flag = false;
            Task<UpdateResult> ur = _Pull<C>(id, filed, expression);
            if (ur != null && ur.Result.ModifiedCount > 0)
            {
                flag = true;
            }
            return flag;
        }
        public Task<T> FindOne(string id,string[] fields)
        {
            var result = _FindOne(id, fields);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public Task<List<T>> Find(string[] fields, Dictionary<string, int> sortfields = null, System.Linq.Expressions.Expression<Func<T, bool>> expression = null)
        {
            return _Find(fields, sortfields, expression);
        }
        #endregion
    }
}
