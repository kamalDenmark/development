using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace TolkRoute1._9
{
    class OrdersByDate
    {
        public List<AllOrders> AllOrderListInOrdersByDate = new List<AllOrders>();
        public OrdersByDate()
        {
            
        
            string source = "server=(local);" + "integrated security=SSPI;" + "database=db_tolkdanmark"; /// Connection string
            string selectOrderPri = "SELECT [order_id],[language],[NumberOfTolks],[order_date],[fromMinutes],[fromHours],[toMinutes],[toHours],[postalCode],[tolkning_info]," +
                                        "[postalCode_tolkning], address, address_tolkning FROM [db_tolkdanmark].[dbo].[OrdersLang] where order_date ='02-05-2016'" +//+DateOfOrders+"'"+
                                        " order by NumberOfTolks,fromHours,fromMinutes"; //,[day],[month],[year]  // this string is used to get orders with language priority and specific date
            SqlConnection conn = new SqlConnection(source);
            conn.Open();
            //----------------------------here we get orders with priority of languages and make their objects
            try
            {
                SqlCommand cmdOrderPri = new SqlCommand(selectOrderPri, conn);
                SqlDataReader readerOrderPri = cmdOrderPri.ExecuteReader();
                while (readerOrderPri.Read())
                {
                    AllOrders currentOrder = new AllOrders(Convert.ToString(readerOrderPri[0]), Convert.ToString(readerOrderPri[1]), Convert.ToString(readerOrderPri[2]),
                                                            Convert.ToString(readerOrderPri[3]), Convert.ToString(readerOrderPri[4]), Convert.ToString(readerOrderPri[5]),
                                                            Convert.ToString(readerOrderPri[6]), Convert.ToString(readerOrderPri[7]), Convert.ToString(readerOrderPri[8]),
                                                            Convert.ToString(readerOrderPri[9]), Convert.ToString(readerOrderPri[10]),
                                                            Convert.ToString(readerOrderPri[11]), Convert.ToString(readerOrderPri[12]));
                    AllOrderListInOrdersByDate.Add(currentOrder); // List is populated with objects of AllOrders class

                }
                readerOrderPri.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(" there is error while opening the conncetion, error is : " + ex);
            }
            conn.Close();
           }

    }
}
