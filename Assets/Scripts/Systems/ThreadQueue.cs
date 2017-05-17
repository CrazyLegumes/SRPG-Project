using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;



public class ThreadJob
{

    Thread job;
    bool done = false;
    object handler = new object();


    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (handler)
            {
                tmp = done;
            }
            return tmp;
        }

        set
        {
            lock (handler)
            {
                done = value;
            }
        }
    }


    ThreadStart Runner;

    public ThreadJob(ThreadStart a)
    {
        Runner = a;




    }

    public virtual void Run()
    {
        ThreadQueue.activeThreads.Add(job);
        ThreadQueue.activeJobs.Add(this);
        Runner();
        IsDone = true;
    }

    public virtual void Start()
    {
        job = new Thread(Run);
       
        job.IsBackground = true;
        job.Start();

    }

    public virtual void Abort()
    {
        IsDone = true;
        job.Abort();
    }

    public virtual void OnFinish()
    {
        
        Debug.Log("Finished Task");
        ThreadQueue.activeThreads.Remove(job);
        ThreadQueue.activeJobs.Remove(this);
        
    }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinish();
            return true;
        }
        return false;
    }

}


public class ThreadQueue : MonoBehaviour
{
    public static List<Action> QueuedActions;
    public static List<Thread> activeThreads;
    public static List<ThreadJob> activeJobs;
    public int threadcount;




    Transform obj;


    Vector3 pos;






    // Use this for initialization
    void Awake()
    {
        QueuedActions = new List<Action>();
        activeThreads = new List<Thread>();
        activeJobs = new List<ThreadJob>();
        //StartThreadFunction(ConcurrencyTest);





    }

    // Update is called once per frame
    void Update()
    {

         for(int i = 0; i < activeJobs.Count; i++)
        {
            activeJobs[i].Update();
        }
            
            
        threadcount = activeThreads.Count;
        while (QueuedActions.Count > 0)
        {
            Action func = QueuedActions[0];
            if (func != null)
                func();
            QueuedActions.RemoveAt(0);

        }




       


    }








    public static void QueueAction(Action function)
    {
        QueuedActions.Add(function);
    }

    public static void StartThreadFunction(Action func)
    {

        ThreadJob t = new ThreadJob(new ThreadStart(func));
        t.Start();


        /*Thread t = new Thread(() =>
        {
            try
            {
                func();

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        });
        activeThreads.Add(t);
        t.IsBackground = true;

        t.Start();
        */


    }









}


