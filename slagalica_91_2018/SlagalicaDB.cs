using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slagalica_91_2018
{
    class SlagalicaDB
    {
        SqlConnection conn = DbConnection.Instance;

        public List<string> rezultati()
        {
            List<string> lista = new List<string>();

            conn.Open();
            SqlCommand command = new SqlCommand("Select * from rezultati", conn);

            List<int> rezs = new List<int>();
            List<string> imes = new List<string>();
            int min, sec;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ime = reader[0].ToString();
                    sec = int.Parse(reader[1].ToString());
                    imes.Add(ime);
                    rezs.Add(sec);
                    
                }
            }

            conn.Close();

            sortiraj(rezs, imes);
            
           
            for (int i = 0; i < rezs.Count; i++)
            {
                min = rezs[i] / 60;
                sec = rezs[i] - min * 60;
                if (imes[i].Length > 12)
                {
                    imes[i] = imes[i].Substring(0, 10)+"...";
                }
                string s = imes[i]+ " (" + min+"min "+sec+"sec)";
                lista.Add(s);
            }
            return lista;
        }

        private void sortiraj(List<int> rezs, List<string> imes)
        {
            int pom;
            string pom2;
            for (int i = 0; i < rezs.Count; i++)
            {
                for (int j = 0; j < rezs.Count; j++)
                {
                    if (rezs[j] > rezs[i])
                    {
                        pom = rezs[i];
                        rezs[i] = rezs[j];
                        rezs[j] = pom;

                        pom2 = imes[i];
                        imes[i] = imes[j];
                        imes[j] = pom2;
                    }
                }
            }
        }

        public void unesiRezultat(string ime, int rezultat, int potezi)
        {
            String query;
            if (postojiIme(ime))
                if (boljiRezultat(ime, rezultat))
                    query = "update rezultati set vreme=@rezultat,potezi=@potezi where ime=@ime";
                else return;
            else
                query = "insert into rezultati values(@ime, @rezultat,@potezi)";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@ime", ime);
                command.Parameters.AddWithValue("@rezultat", rezultat);
                command.Parameters.AddWithValue("@potezi", potezi);

                conn.Open();
                int result = command.ExecuteNonQuery();
                conn.Close();
               
                if (result < 0)
                    Console.WriteLine("Error inserting/updating data into Database!");
            }

        }

        private bool boljiRezultat(string ime, int rezultat)
        {
            conn.Open();
            SqlCommand command = new SqlCommand("Select vreme from rezultati where ime='"+ime+"'", conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            
            if (int.Parse(reader[0].ToString()) > rezultat)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        private bool postojiIme(string ime)
        {
            conn.Open();
            SqlCommand command = new SqlCommand("Select * from rezultati where ime='"+ime+"'", conn);
            object reader = command.ExecuteScalar();
            conn.Close();
            if (reader != null)
                return true;
            return false;
        }
    }
}
