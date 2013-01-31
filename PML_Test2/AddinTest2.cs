using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aveva.ApplicationFramework;
using Aveva.Pdms.Database;
using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Shared;
using Aveva.Pdms.Utilities;
using System.Windows.Forms;
using System.Drawing;
namespace PML_Test2
{
    //A classe deve herdar de Iaddin, pois quando o Addin for carregado no PDMS, o método Start() será executado
    public class AddinTest2 : IAddin
    {
        public string Description
        {
            get
            {
                return "Um addin para teste";
            }
        }
        public string Name
        {
            get
            {
                return "AddinTest";
            }
        }

        public void Start(ServiceManager serviceManager)
        {
            //Criando comando
            CommandManager commandManager = (CommandManager)serviceManager.GetService(typeof(CommandManager));
            ComandoTeste test = new ComandoTeste();
            commandManager.Commands.Add(test); 

            //Vamos criar uma barra de comandos
            CommandBarManager commandBarManager = (CommandBarManager)serviceManager.GetService(typeof(CommandBarManager));
            CommandBar commandBar = commandBarManager.CommandBars.AddCommandBar("MinhaBarraDeComando");
            
            RootToolsCollection rtc = commandBarManager.RootTools;
            //Criando um botão que executa o comando criado nesse addin
            ButtonTool button = rtc.AddButtonTool("Botao", "Adicionar Nova Revisão", null, "MeuTeste");
            button.Enabled = true;
            button.Visible = true;
            //Adicionando um botão à nossa barra de comandos
            commandBar.Tools.AddTool("Botao");
        }


        public void Stop()
        {
        }
    }
}
