using System;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

namespace currency
{
	class Program
	{
		public static void Main(string[] args)
		{
			DateTime now = DateTime.Now;
			string a="http://www.cbr.ru/scripts/XML_daily.asp?date_req="+now.ToString("d");
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(a);
			XmlElement xRoot = xDoc.DocumentElement;
			const string Connect = "Database=kurs;Data Source=localhost;User Id=root;Password=1";
			using (SqlConnection conn = new SqlConnection(Connect))
            {
				conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
			foreach(XmlNode xnode in xRoot)
        {
				cmd.CommandText = @"INSERT INTO Курс(дата, валюта, курс)
                                    VALUES(
                                           @date, 
                                           @val, 
                                           @cur";
				cmd.Parameters.Add("@date", SqlDbType.DateTime);
				cmd.Parameters["@date"].Value = now;
				foreach(XmlNode childnode in xnode.ChildNodes)
            {
					if(childnode.Name=="Value")
                {
						cmd.Parameters.Add("@val", SqlDbType.Int);
						cmd.Parameters["@val"].Value = childnode.Value;
                }
					if(childnode.Name=="CharCode")
                {
						cmd.Parameters.Add("@cur", SqlDbType.VarChar);
						cmd.Parameters["@cur"].Value = childnode.Value;
                }
            }
				cmd.ExecuteNonQuery();
			}
			conn.Close();
			}
    }
	}
}