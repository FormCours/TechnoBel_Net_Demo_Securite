using Demo_Securite.DAL.Entities;
using Demo_Securite.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Securite.DAL.Services
{
    public class MemberService : IMemberService
    {
        private IDbConnection _Connection;
        public MemberService(IDbConnection connection)
        {
            _Connection = connection;
        }

        private Member MapToMember(IDataRecord data)
        {
            return new Member
            {
                Id = (int)data["Id"],
                Username = data["Username"].ToString(),
                Email = data["Email"].ToString(),
                Password = null,
                Salt = null,
                Role = data["Role"].ToString()
            };
        }

        private IDbDataParameter CreateDbParameter(IDbCommand cmd, string name, object value)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        public IEnumerable<Member> Get()
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT M.Id, M.Username, M.Email, R.Name AS [Role] " +
                "FROM Member M " +
                "     JOIN Role R ON M.RoleId = R.Id";

                cmd.CommandType = CommandType.Text;

                try
                {
                    _Connection.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return MapToMember(reader);
                        }
                    }
                }
                finally
                {
                    _Connection.Close();
                }
            }
        }

        public Member GetById(int id)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT M.Id, M.Username, M.Email, R.Name AS [Role] " +
                "FROM Member M " +
                "     JOIN Role R ON M.RoleId = R.Id " +
                "WHERE M.Id = @Id";

                IDbDataParameter parameter = CreateDbParameter(cmd, "Id", id);

                cmd.Parameters.Add(parameter);
                cmd.CommandType = CommandType.Text;

                try
                {
                    _Connection.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasRow = reader.Read();
                        return !hasRow ? null : MapToMember(reader);
                    }
                }
                finally
                {
                    _Connection.Close();
                }
            }
        }

        public int Create(Member entity)
        {
            using(IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [Member] (Username, Email, Password, Salt) " +
                                  " OUTPUT inserted.Id " +
                                  " VALUES (@Username, @Email, @Password, @Salt);";

                byte[] pwdBytes = Encoding.ASCII.GetBytes(entity.Password);

                IDbDataParameter username = CreateDbParameter(cmd, "Username", entity.Username);
                IDbDataParameter email = CreateDbParameter(cmd, "Email", entity.Email);
                IDbDataParameter password = CreateDbParameter(cmd, "Password", pwdBytes);
                IDbDataParameter salt = CreateDbParameter(cmd, "Salt", entity.Salt);

                cmd.Parameters.Add(username);
                cmd.Parameters.Add(email);
                cmd.Parameters.Add(password);
                cmd.Parameters.Add(salt);

                try
                {
                    _Connection.Open();
                   return (int)cmd.ExecuteScalar();
                }
                finally
                {
                    _Connection.Close();
                }
            }
        }

        public Member Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Member Update(int id, Member entity)
        {
            throw new NotImplementedException();
        }

        public MemberCredential GetCredential(string email)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Salt, Password " +
                                  "FROM Member " +
                                  "WHERE Email = @Email";
                cmd.CommandType = CommandType.Text;

                IDbDataParameter parameter = CreateDbParameter(cmd, "Email", email);
                cmd.Parameters.Add(parameter);

                try
                {
                    _Connection.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasRow = reader.Read();
                        return !hasRow ? null : new MemberCredential
                        {
                            Salt = reader["Salt"].ToString(),
                            HashPassword = Encoding.ASCII.GetString((byte[])reader["Password"])
                        };
                    }
                }
                finally
                {
                    _Connection.Close();
                }
            }
        }

        public Member GetByEmail(string email)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT M.Id, M.Username, M.Email, R.Name AS [Role] " +
                                  "FROM Member M " +
                                  "     JOIN Role R ON M.RoleId = R.Id " +
                                  "WHERE M.Email = @email";
                cmd.CommandType = CommandType.Text;

                IDbDataParameter parameterMail = CreateDbParameter(cmd, "Email", email);
                cmd.Parameters.Add(parameterMail);

                try
                {
                    _Connection.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasRow = reader.Read();
                        return !hasRow ? null : MapToMember(reader);
                    }
                }
                finally
                {
                    _Connection.Close();
                }
            }
        }
    }
}
