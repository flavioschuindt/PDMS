using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Database;
using System.Windows.Forms;
using Aveva.Pdms.Shared;

namespace PML_Test2
{
    class ComandoTeste : Command
    {
        public ComandoTeste()
        {
            this.Key = "MeuTeste";
        }

        

        public override void Execute()
        {

            //Ler linhas da lista de pipes
            string[] pipes = System.IO.File.ReadAllLines(@"V:\Automacao_de_Projetos\Trabalho\Desenvolvimento\AutomacaoPDMS\Macros\lista.txt");

            foreach (string pipe in pipes)
            {
                CreateNewRev(pipe);
            }
        }

        private void CreateNewRev(string pipe)
        {
            //Selecionando o pipe
            DbElement elem = DbElement.GetElement(pipe);
            CurrentElement.Element = elem;
            elem.Claim();

            //Pegando a data
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            today = today.Replace('/', '.');

            string[] templateRev = { "A", "0", "1", "2" };

            List<string> udas = new List<string>(); /* udas = user defined attributes*/
            udas.Add(":NewRevi");
            udas.Add(":Revi1");
            udas.Add(":Revi2");
            udas.Add(":Revi3");

            List<DbAttribute> Revisao = GetUserDefinedAttributes(udas);

            udas.Clear();
            udas.Add(":NewDate");
            udas.Add(":Date1");
            udas.Add(":Date2");
            udas.Add(":Date3");

            List<DbAttribute> Data = GetUserDefinedAttributes(udas);

            int LastRev = 0;
            foreach (DbAttribute rev in Revisao)
            {
                string attrValue = elem.GetAsString(rev);
                if (attrValue != "" && attrValue != " " && attrValue != "unset" && attrValue != "REV.")
                {
                    LastRev++;
                }
            }



            if (LastRev <= Revisao.Count - 1)
            {
                int i = LastRev - 1;
                for (int k = 0; k <= LastRev - 1; k++)
                {
                    if (k < i)
                    {
                        
                        String aux = elem.GetAsString(Revisao[k]);
                        elem.SetAttribute(Revisao[k], elem.GetAsString(Revisao[i]));
                        elem.SetAttribute(Revisao[i], aux);

                        aux = elem.GetAsString(Data[k]);
                        elem.SetAttribute(Data[k], elem.GetAsString(Data[i]));
                        elem.SetAttribute(Data[i], aux);
                        i--;
                    }
                }

                elem.SetAttribute(Revisao[LastRev], templateRev[LastRev]);
                elem.SetAttribute(Data[LastRev], today);

                i = LastRev;
                for (int k = 0; k <= LastRev; k++)
                {
                    if (k < i)
                    {
                        String aux = elem.GetAsString(Revisao[k]);
                        elem.SetAttribute(Revisao[k], elem.GetAsString(Revisao[i]));
                        elem.SetAttribute(Revisao[i], aux);

                        aux = elem.GetAsString(Data[k]);
                        elem.SetAttribute(Data[k], elem.GetAsString(Data[i]));
                        elem.SetAttribute(Data[i], aux);
                        i--;
                    }
                }
            }
            else
            {
                MessageBox.Show("Todas as revisoes do pipe" + pipe + "estao preenchidas");
            }

        }

        private List<DbAttribute> GetUserDefinedAttributes(List<string> attributes)
        {
            List<DbAttribute> udas = new List<DbAttribute>();
            foreach (string attribute in attributes)
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(attribute);
                udas.Add(uda);
            }

            return udas;
        }
        
    }
}
