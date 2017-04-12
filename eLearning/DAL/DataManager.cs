using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DataManager
    {
        SqlConnection myConnection = new SqlConnection("user id=sa;" +
                                       "password=letmein99;server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=eLearning; " +
                                       "connection timeout=30");

        public DataTable SelectUrlListByField(int fieldId)
        {
            DataTable dtUrlList = new DataTable();
            try
            {
                myConnection.Open();
                string query = "SELECT * FROM url_list WHERE field_id=" + fieldId;
                SqlCommand cmd = new SqlCommand(query, myConnection);
                
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dtUrlList);
                myConnection.Close();
                da.Dispose();
            }
            catch (Exception ex)
            { 
            
            }
            return dtUrlList; 
        }

        public DataTable SelectFields()
        {
            DataTable dtField = new DataTable();
            try
            {
                myConnection.Open();
                string query = "SELECT * FROM fields";
                SqlCommand cmd = new SqlCommand(query, myConnection);

                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dtField);
                myConnection.Close();
                da.Dispose();
            }
            catch (Exception ex)
            {

            }
            return dtField;
        }

        public string InsertUrl(int fieldId, string url )
        {
            string result = "";
            try
            {
                myConnection.Open();
            
                string insQuery = "INSERT INTO url_list (field_id, url) " +
                                         "Values (" + fieldId + ", '" + url + "')";
                SqlCommand myCommand = new SqlCommand(insQuery, myConnection);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = "Success: Url added";
            }
            catch (Exception e)
            {
                result = "Fail: " + e.Message;
            }
            
            return result;
        }

        public string DeleteUrl(int id)
        {
            string result = "";
            try
            {
                myConnection.Open();

                string delQuery = "DELETE FROM url_list WHERE id = " + id;
                SqlCommand myCommand = new SqlCommand(delQuery, myConnection);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
                result = "Success: Url deleted";
            }
            catch (Exception ex)
            {
                result = "Fail: " + ex.Message;
            }
            return result;
        }

        public List<string> SelectAllAuthenticUrls()
        {
            List<string> list_urls = new List<string>();
            DataTable dtUrls = new DataTable();
            try
            {
                myConnection.Open();
                string query = "SELECT * FROM url_list";
                SqlCommand cmd = new SqlCommand(query, myConnection);

                // create data adapter
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                // this will query your database and return the result to the list of string
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr == null)
                    {
                        throw new NullReferenceException("Fail: No url Available.");
                    }
                    while (rdr.Read())
                    {
                        list_urls.Add(rdr["url"].ToString());
                    }
                }

                //da.Fill(dtUrls);
                //myConnection.Close();
                //da.Dispose();
            }
            catch (Exception ex)
            {

            }

            //if (dtUrls != null && dtUrls.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dtUrls.Rows)
            //    {
            //        list_urls.Add(dr["url"].ToString());
            //    }
            //}
            return list_urls;
        }

    }
}
