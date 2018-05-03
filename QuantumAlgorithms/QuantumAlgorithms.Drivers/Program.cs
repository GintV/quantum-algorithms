namespace QuantumAlgorithms.Drivers
{
    class Program
    {
        static void Main(string[] args)
        {
            //var result = new IntegerFactorizationDriver(new ConsoleLogger()).Run(33);
            //var result = DoWork("");
            //Console.WriteLine(result);
            //Console.ReadLine();
        }

        //delegate (bool, (int, int)) WorkDelegate(string arg);
        //public static (bool, (int, int)) DoWork(string arg)
        //{
        //    WorkDelegate d = DoWorkHandler;
        //    IAsyncResult res = d.BeginInvoke(arg, null, null);
        //    if (res.IsCompleted == false)
        //    {
        //        res.AsyncWaitHandle.WaitOne(2000, false);
        //        if (res.IsCompleted == false)
        //            throw new ApplicationException("Timeout");
        //    }
        //    return d.EndInvoke(res);
        //}
        //private static (bool, (int, int)) DoWorkHandler(string arg)
        //{
        //    return new IntegerFactorizationDriver(new ConsoleLogger()).Run(33);
        //}
    }
}