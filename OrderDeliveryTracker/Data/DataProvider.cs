using OrderDeliveryTracker.Helpers;
using OrderDeliveryTracker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OrderDeliveryTracker.Data
{
    public class DataProvider
    {
        private static string connectionString = string.Empty;

        public DataProvider()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }

        public User Login(LoginModel _loginUser)
        {
            User _user = new User();            
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spUserLogin", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@userName", SqlDbType.VarChar).Value = _loginUser.UserName;
                        command.Parameters.Add("@password", SqlDbType.VarChar).Value = Crypto.Encrypt(_loginUser.Password);

                        SqlParameter id = new SqlParameter("@outStatus", SqlDbType.Int);
                        id.Direction = ParameterDirection.Output;
                        command.Parameters.Add(id);

                        SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter(command);
                        DataSet _DataSet = new DataSet();
                        try
                        {
                            _SqlDataAdapter.Fill(_DataSet);
                            int outStatus = (int)id.Value;
                            _user.LoginOutput = (LoginOutput)outStatus;
                            if (_DataSet.Tables.Count> 0 && _DataSet?.Tables[0] != null && _DataSet.Tables[0]?.Rows?.Count != 0)
                            {
                                DataRow _DataRow = _DataSet.Tables[0].Rows[0];
                                _user.UserName = _DataRow["UserName"]?.ToString();
                                _user.IsAdmin = Convert.ToBoolean(_DataRow["IsAdmin"]?.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            _user.LoginOutput = LoginOutput.Error;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _user.LoginOutput = LoginOutput.Error;
            }            
            return _user;
        }

        public CreateUserOutput SignUp(CreateUser _user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spSignUpUser", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@userName", SqlDbType.VarChar).Value = _user.UserName;
                        command.Parameters.Add("@password", SqlDbType.VarChar).Value = Crypto.Encrypt(_user.Password);
                        
                        SqlParameter id = new SqlParameter("@outStatus", SqlDbType.Int);
                        id.Direction = ParameterDirection.Output;
                        command.Parameters.Add(id);

                        if (con.State == ConnectionState.Open)
                            con.Close();
                        con.Open();
                        command.ExecuteNonQuery();
                        int outStatus = (int)id.Value;
                        return (CreateUserOutput)outStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                return CreateUserOutput.Error;
            }
        }

        public List<Order> GetOrders(string userName, bool isAdmin)
        {
            List<Order> _orders = new List<Order>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetOrders", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@userName", SqlDbType.VarChar).Value = userName;
                        command.Parameters.Add("@isAdmin", SqlDbType.Bit).Value = isAdmin;

                        SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter(command);
                        DataSet _DataSet = new DataSet();
                        try
                        {
                            _SqlDataAdapter.Fill(_DataSet);
                            
                            if (_DataSet.Tables.Count > 0 && _DataSet?.Tables[0] != null && _DataSet.Tables[0]?.Rows?.Count != 0)
                            {
                                foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                                {
                                    Order _order = new Order();
                                    _order.OrderID = Convert.ToInt64(_DataRow["ID"]?.ToString());
                                    _order.OrderName = _DataRow["OrderName"]?.ToString();
                                    _order.AssignTo = _DataRow["UserName"]?.ToString();
                                    _order.Status = _DataRow["Status"]?.ToString();
                                    _orders.Add(_order);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _orders;
        }

        public bool DeliveryUpdate(long orderId)
        {
            bool isSuccess = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spUpdateDelivery", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@orderId", SqlDbType.BigInt).Value = orderId;

                        SqlParameter id = new SqlParameter("@outStatus", SqlDbType.Int);
                        id.Direction = ParameterDirection.Output;
                        command.Parameters.Add(id);

                        if (con.State == ConnectionState.Open)
                            con.Close();
                        con.Open();
                        command.ExecuteNonQuery();
                        int outStatus = (int)id.Value;
                        if (outStatus > 0)
                            isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isSuccess;
        }



    }
}