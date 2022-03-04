﻿using Dooggy;
using Dooggy.Factory;
using Dooggy.Lib.Generic;
using Dooggy.Lib.Vars;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dooggy.Factory.Console
{
    public class TestResult
    {

        private TestScript Script;

        private TestConsoleInput Input => Script.Console.Input;

        public string name_INI;

        public string name_OUT;

        private string _codeZero;

        private string _code;
        private string _data;

        public string code { get => _code; }
        public string data { get => _data; }

        public bool IsError => Log.IsError;
        public bool IsData => (myString.IsFull(data));

        public bool IsDataSaved;
        public bool IsChanged => (myString.IsNoEqual(_code, _codeZero));

        public TestResultLog Log;

        public TestResultBase Err => Log.Err;
        public TestResultSQL SQL => Log.SQL;

        public TestResult(TestScript prmScript)
        {

            Script = prmScript;

            Log = new TestResultLog();

        }

        public void Setup(string prmCode, string prmArquivoINI)
        {

            SetSave(prmCode);

            name_INI = prmArquivoINI;

        }

        public void LogStart(string prmArquivoOUT) { Log.Start(); name_OUT = prmArquivoOUT; IsDataSaved = false; }
        public void LogStop() => Log.Stop();

        public void AddLogItem(string prmTipo, string prmTexto) => Log.AddItem(prmTipo, prmTexto);
        public void AddLogSQL(string prmSQL, long prmTimeElapsed) => Log.AddSQL(prmSQL, prmTimeElapsed);
        public void SetSave(string prmCode) { _codeZero = prmCode; SetCode(_codeZero); }
        public void SetCode(string prmCode) => _code = prmCode;
        public void SetData(string prmData) { _data = prmData; IsDataSaved = true; }
        public void UndoCode() => SetSave(prmCode: Script.Console.Input.GetCode(name_INI));

    }
    public class TestResultLog
    {

        public bool ativo;

        public TestResultMain Main;

        public TestResultSQL SQL;

        public TestResultBase Err => Main.Err;

        public string txt => Main.txt;

        public bool IsError => Err.IsFull;

        public TestResultLog()
        {
            Clear();
        }

        private void Clear()
        {
            Main = new TestResultMain(); SQL = new TestResultSQL();
        }

        public void Start() { Clear(); ativo = true; }
        public void AddItem(string prmTipo, string prmTexto)
        {
            if (ativo)
            {

                TestItemLog item = new TestItemLog(prmTipo, prmTexto);

                Main.Add(item);

            }

        }
        public void AddSQL(string prmSQL, long prmTimeElapsed)
        {
            if (ativo)
            {

                TestItemLog item = new TestItemLog(prmSQL, prmTimeElapsed);

                SQL.Add(item);

            }

        }
        public void Stop() => ativo = false;

    }

    public class TestResultMain : TestResultBase
    {

        public TestResultBase Err => GetErr();

        private TestResultBase GetErr()
        {

            TestResultBase result = new TestResultBase();

            foreach (TestItemLog item in this)
                if (item.IsErr)
                    result.Add(item);

            return (result);
        }

    }

    public class TestResultSQL : TestResultBase
    {
        public string log => GetLog();

        private string GetLog()
        {

            xMemo log = new xMemo();

            foreach (TestItemLog item in this)
                log.Add(item.time);

            return (log.memo_ext);
        }
    }

    public class TestResultBase : List<TestItemLog>
    {
        public string txt => GetTXT();
        public bool IsFull => (this.Count > 0);

        private string GetTXT()
        {

            xMemo log = new xMemo();

            foreach (TestItemLog item in this)
                log.Add(item.msg);

            return (log.memo_ext);
        }

    }
}