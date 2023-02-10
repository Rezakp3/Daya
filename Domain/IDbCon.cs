namespace Domain
{
    public interface IDbCon
    {
        public int Execute<T>(string sql,T dt);
        public int Execute(string sql);
        public T Query<T>(string sql);
        public List<T> QueryList<T>(string sql);
        public List<T> CallStoredProsedure<T>(string sql, Dictionary<string,dynamic> parameters);
    }
}
