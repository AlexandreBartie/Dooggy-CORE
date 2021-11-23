﻿using Dooggy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Dooggy.KERNEL
{
    public class TestKernel
    {

        public TestTrace Trace = new TestTrace();

        public TestConfig Config = new TestConfig();

        public string GetProjectBlockCode() => ("BASE, DATA, BUILD, CONFIG");
        public string GetScriptBlockCode() => ("PLAY, CHECK, CLEANUP");
        public string GetAdicaoElementos() => ("+");

        public string GetXPathBuscaRaizElementos() => "//*[@{0}='{1}']";

        public bool Call(Object prmObjeto, string prmMetodo)
        {

            bool vlOk = true;

            xLista lista = new xLista();

            lista.Parse(prmMetodo, prmSeparador: ",");

            foreach (string metodo in lista)
            {
                try
                {
                    prmObjeto.GetType().GetMethod(metodo).Invoke(prmObjeto, null);
                }

                catch
                {

                    Trace.Log.Aviso(string.Format("Método [{0}.{1}] não encontrado. [ error: {2} ]", prmObjeto.GetType().Name, metodo, prmObjeto.GetType().FullName));

                    vlOk = false;

                }

            }

            return (vlOk);

        }
        public void Pause(int prmSegundos)
        { Thread.Sleep(TimeSpan.FromSeconds(prmSegundos)); }

    }
    public class TestConfig
    {

        public string PathFileSources;

        public Encoding EncodedDataJUNIT;

        public bool OnlyDATA;

        public int PauseAfterTestCase;

        public int PauseAfterTestScript;

        public int PauseAfterTestSuite;

    }

    public class TestTrace
    {

        public TestLog Log;

        public TestTraceDataBase DataBase;

        public TestTraceAction Action;

        public TestTrace()
        {

            Log = new TestLog();

            DataBase = new TestTraceDataBase(this);

            Action = new TestTraceAction(this);

        }





        // public bool ActionFail(string prmErro, string prmDestaque) => (Log.Erro(String.Format("{0} ... [{1}]", prmErro, prmDestaque)));


        //public void NoFindMetodh(string prmAcao, string prmElemento, string prmValor)

        //Log.Aviso(string.Format("Método {0}.{1} não encontrado. [{2}]





        //

    }

    public class TestTraceDataBase
    {

        private TestTrace Trace;

        public TestLog Log { get => Trace.Log; }

        public TestTraceDataBase(TestTrace prmTrace)
        {

            Trace = prmTrace;

        }

        public void StatusConnection(string prmTag, string prmStatus) { Log.SQL(string.Format("Banco de Dados {1}: tag[{0}]", prmTag, prmStatus)); }
        public void SQLConnection(string prmTag, string prmSQL) => Log.SQL(string.Format("DataSet criado com sucesso: { tag[{0}] }", prmTag));

        public void FailConnection(string prmTag, string prmStringConexao, Exception prmErro) => Log.Erro(String.Format("Falha na Conexão do Banco de Dados ... tag:[{0}] string: {1}", prmTag, prmStringConexao),prmErro);

        public void FailSQLConnection(string prmTag, string prmSQL, Exception prmErro) => Log.Erro(String.Format("Falha no comando SQL ... tag:[{0}] string: {1}", prmTag, prmSQL), prmErro);

    }
    public class TestTraceAction
    {

        private TestTrace Trace;

        public TestLog Log { get => Trace.Log; }

        public TestTraceAction(TestTrace prmTrace)
        {

            Trace = prmTrace;

        }
        public void ActionArea(string prmArea, string prmName) => Log.Trace(String.Format("{0,20}: [{1}]", prmArea, prmName));

        public bool ActionMassaOnLine(bool prmMassaOnLine)
        {

            string texto;

            if (prmMassaOnLine)
                texto = "ON-LINE";
            else
                texto = "OFF-LINE";

            Log.Trace(String.Format("Massa de Testes '{0}'", texto));

            return (prmMassaOnLine);

        }

        public void ActionElement(string prmAcao, string prmElemento) => ActionElement(prmAcao, prmElemento, prmValor: null);
        public void ActionElement(string prmAcao, string prmElemento, string prmValor)
        {

            string msg = String.Format("ACTION: {0} {1,15} := {1}", prmAcao, prmElemento);

            if (prmValor != null)
                msg += " := " + prmValor;

            Log.Trace(msg);

        }

        public void ActionFail(string prmComando, Exception e) => Log.Erro("ACTION FAIL: ROBOT." + prmComando, e);

        public void TargetNotFound(string prmTAG) => Log.Erro("TARGET NOT FOUND: " + prmTAG);

    }

    public class TestLog
    {

        public bool Interno(string prmErro) => Message("KERNEL", prmErro);
        public bool Trace(string prmTrace) => Message("TRACE", prmTrace);
        public bool SQL(string prmMensagem) => Message("SQL", prmMensagem);
        public bool Show(string prmMensagem) => Message("SHOW", prmMensagem);
        public bool Aviso(string prmAviso) => Message("AVISO", prmAviso);
        public bool Falha(string prmAviso) => Message("FALHA", prmAviso);
        public bool Erro(string prmErro) => Message("ERRO", prmErro);
        public bool Erro(Exception e) => Message("ERRO", e.Message);
        public bool Erro(string prmErro, Exception e) => Message("ERRO", String.Format("{0} Error: {1}", prmErro, e.Message));


        private bool Message(string prmTipo, string prmMensagem)
        {
            Debug.WriteLine(String.Format("[{0,5}]: {1} ", prmTipo, prmMensagem));
            return false;
        }

    }

}