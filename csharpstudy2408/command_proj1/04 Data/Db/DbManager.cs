using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace command_proj1._04_Data.Db
{
    public class DbManager
    {
        private const string _connectionString = "host=10.10.10.108;port=45111;username=ecount_user;password=1q2w3e4r;database=ecount";

        public DbManager()
        {

        }

        // Fetch : 단건, 한 로우 조회
        // Query: 여러 줄 쿼리
        // Execute: 실행(인서트,업데이트) 결과값 x
        // Scalar: 단 한 값 

        public int Execute(string sql, Dictionary<string, object> parameters)
        {
            // using => 실제로 Db 서버와 연결할건데, 만약 exeception이 날아왔다 => 직접 try catch 잡기 => 모든 커넥션에 그 로직 다 넣어줘야 함
            // => 그게 귀찮아서 내가 여기서 이 블락 끝나면 이 객체에 Dispose (IDisposable) 호출 시켜줘라는 뜻.
            // => using 문 사용하면 이 블락 끝나면 C#은 Dispose를 호출해줄게 라는 뜻 ㅎ 결국 하는 일은 커넥션 클로즈 해주게 되는 것.
            using (var conn = new NpgsqlConnection(_connectionString)) {
                conn.Open();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = sql;
                    foreach (var param in parameters) {
                        cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
                    }

                    return cmd.ExecuteNonQuery(); // 쿼리 실행 후 영향 받은 로우의 수를 반환해준다.
                }
            }
        }

        public List<T> Query<T>(string sql, Dictionary<string, object> parameters, Action<DbDataReader, T> mapper) // 일일히 매핑을 시켜줘야함. 그냥 데이터를 가져온 것이지.
            where T : new() // T 타입을 가지고도 new를 할 수 있게 해주는.
        {
            using (var conn = new NpgsqlConnection(_connectionString)) {
                conn.Open();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = sql;
                    foreach (var param in parameters) {
                        cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
                    }
                    var result = new List<T>();
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) // bool 하나씩 읽어오는데 읽을게 없으면 펄즈.
                        {
                            var data = new T();
                            // var a = reader["com_code"] 이렇게 뽑을 수도 있고 mapper로 밖에서 어떻게 매핑할지 결정하도록 할 수 있고.
                            mapper(reader, data);
                            result.Add(data);
                        }
                    }
                    return result;
                }
            }
        }

        public T Scalar<T>(string sql, Dictionary<string, object> parameters, Func<DbDataReader, T> mapper) where T : new()
        {
            using (var conn = new NpgsqlConnection(_connectionString)) {
                conn.Open();
                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = sql;
                    foreach (var param in parameters) {
                        cmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value));
                    }
                    T result = default(T);
                    using (var reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            result = new T();
                            return mapper(reader);
                        }
                        throw new Exception("No result");
                    }
                    return default(T);
                }
            }
        }
    }
}
