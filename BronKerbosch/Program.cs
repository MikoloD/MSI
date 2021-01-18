using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Antlr.Runtime;
using Graphviz4Net;
using Graphviz4Net.Dot;
using Graphviz4Net.Dot.AntlrParser;
using Graphviz4Net.Graphs;

namespace BronKerbosch
{
    class Program
    {
        public static DotGraph<int> Graf { get; set; }
        public IntPtr MaxWorkingSet { get; set; }
        [STAThread]
        static void Main(string[] args)
        {
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            Process.GetCurrentProcess().MaxWorkingSet = new IntPtr(1000000000);
            Thread thread = new Thread(delegate ()
            {
                ThreatImplementation(args);
            }, 100000000);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            sw1.Stop();
            Console.WriteLine(sw1.Elapsed);
        }
        public static List<List<int>> Run(List<int> R, List<int> P, List<int> X, List<List<int>> maximalCliques)
        {
            if (!P.Any() && !X.Any() && R.Count() > 1) maximalCliques.Add(R);
            else
            {
                List<int> nodesCopy = new List<int>();
                foreach (var elem in P) nodesCopy.Add(elem);

                foreach (var v in nodesCopy)
                {
                    List<int> GrafV = new List<int>();
                    GrafV.Add(v);

                    List<int> neighborGraph = new List<int>();
                    foreach (var elem in Graf.VerticesEdges)
                    {
                        if (elem.Source.Id == v) neighborGraph.Add(elem.Destination.Id);
                        else if (elem.Destination.Id == v) neighborGraph.Add(elem.Source.Id);
                    }
                    
                    maximalCliques = Run(R.Union(GrafV).ToList(), P.Intersect(neighborGraph).ToList(), X.Intersect(neighborGraph).ToList(), maximalCliques);

                    P.Remove(v);
                    X.Add(v);
                };
            }
            return maximalCliques;
        }
        static void ThreatImplementation(string[] args)
        {
            
            List<List<int>> maximalCliques = new List<List<int>>();
            StreamReader sr = new StreamReader(args[0]);
            Graf = Parse(sr.ReadToEnd());
            List<int> Vertices = new List<int>();
           foreach(var elem in Graf.AllVertices)
                {
                    Vertices.Add(elem.Id);
                };
            Run(new List<int>(), Vertices, new List<int>(), maximalCliques);
            foreach(List<int> elem in maximalCliques)
            {
                foreach(int item in elem)
                {
                    Console.Write(item+" ");
                }
                Console.WriteLine();
            }
           
        }

        public static DotGraph<int> Parse(string content)
        {
            ANTLRStringStream antlrStream = new ANTLRStringStream(content);
            DotGrammarLexer lexer = new DotGrammarLexer(antlrStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            DotGrammarParser parser = new DotGrammarParser(tokenStream);
            IntDotGraphBuilder builder = new IntDotGraphBuilder();
            parser.Builder = builder;
            parser.dot();
            return builder.DotGraph;
        }
    }

    public abstract class GraphBase
    {
        protected readonly int numVertices;

        public GraphBase(int numVertices)
        {
            this.numVertices = numVertices;
        }

        public abstract void AddEdge(int v1, int v2, int weight);

        public abstract IEnumerable<int> GetAdjacentVertices(int v);

        public int NumVertices { get { return numVertices; } }
    }
}
