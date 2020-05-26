using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TolkRoute1._9
{
    class PostCode
    {
        public PostCode parent;
        public string lable;
        public Hashtable PostCodeHash;

        string source = "server=(local);" + "integrated security=SSPI;" + "database=db_tolkdanmark"; /// Connection string
        string selectLanguage = " select interpreter_id, name,language_id from TolkLang";

        public List<TolkDetails> ActiveTolkInPcList = new List<TolkDetails>();
        List<Language> LanguageListInPc = new List<Language>();

        //Program copyTolkPostCode = new Program(); // you can copy tolkdetailist from program file. so there is no need of addTolk method. you can copy ActiveTolkDetailList and populate 
        // ActiveTolkInPcList and then you can add languages by using addLangToTolk method. so there is no need of addTolk mehtod.


        public PostCode(string Ppostcode)
        {
            addTolk(Ppostcode);
            addTolkLang();
            addLangToTolk();
            lable = Ppostcode;
            PostCodeHash = new Hashtable();
            parent = null;
        }
        public void addExitPostCode(PostCode p)
        {
            PostCodeHash.Add(p.lable, p);
        }

        public void addTolk(string pcInaddtolkFun)
        {
            SqlConnection conn = new SqlConnection(source);
            conn.Open();
            string selectTolkDetail = "select interpreter_id, name, Priority, postalCode, isActive from ActiveTolk where postalCode= '" + pcInaddtolkFun + "'"; // this string is used for active Tolks detail
            SqlCommand cmdTolkDetail = new SqlCommand(selectTolkDetail, conn);
            SqlDataReader readerTolkDetail = cmdTolkDetail.ExecuteReader();
            //int i = 0;
            while (readerTolkDetail.Read())
            {
                TolkDetails currentTolk = new TolkDetails(Convert.ToString(readerTolkDetail[0]), Convert.ToString(readerTolkDetail[1]), Convert.ToInt32(Convert.ToString(readerTolkDetail[2])),
                                                          Convert.ToString(readerTolkDetail[3]), Convert.ToString(readerTolkDetail[4]));
                if (currentTolk.cTpostalCode == pcInaddtolkFun)
                {
                    ActiveTolkInPcList.Add(currentTolk); // List is populated with objects of ActiveTolkDetail class
                    //Console.WriteLine(" the tolk added is : " + currentTolk.cTname+" and i is "+i);
                    //i++;
                }
            }
            readerTolkDetail.Dispose();
            conn.Close();
        }

        public void addTolkLang()
        {
            SqlConnection conn = new SqlConnection(source);
            conn.Open();
            SqlCommand cmdLanguage = new SqlCommand(selectLanguage, conn);
            SqlDataReader readerLanguage = cmdLanguage.ExecuteReader();
            while (readerLanguage.Read())
            {
                Language currentLanguage = new Language(Convert.ToString(readerLanguage[0]), Convert.ToString(readerLanguage[1]), Convert.ToString(readerLanguage[2]));
                LanguageListInPc.Add(currentLanguage); // 
            }
            readerLanguage.Dispose();
            conn.Close();
        }
        public void addLangToTolk()
        {
            for (int i = 0; i < ActiveTolkInPcList.Count; i++)
            {
                for (int j = 0; j < LanguageListInPc.Count; j++)
                {
                    if (ActiveTolkInPcList[i].cTinterpreter_id == LanguageListInPc[j].cLinterpreter_id)
                    {
                        ActiveTolkInPcList[i].cTlangList.Add(LanguageListInPc[j].cLname);
                    }
                }
            }
        }
    }
}
