﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public interface IDao
    {
        // Users -----------------------------------------------
        void CreateUser(string username, string password, string email);

        bool CheckCredential(string username, string password);

        bool IsUsernameInUse(string username);

        bool IsEmailInUse(string username);

        string GetUsername(string email);

        string GetPassword(string username);

        void ResetPassword(string username, string password, string email);


        // Notes -----------------------------------------------
        ObservableCollection<MyNote> GetAllNote();

        void SaveNote(MyNote note);

        Task OpenNote(MyNote note);

        void DeleteNote(MyNote note);

        void RenameNote(MyNote note);

        int CreateNote(string noteName);


        // Tasks ----------------------------------------------
        ObservableCollection<MyTask> GetAllTasks();
        void InsertTask(MyTask task);
        void DeleteTask(MyTask task);
        void UpdateTask(MyTask task);
        ObservableCollection<MyTask> GetTasksForDate(DateTime date);
        ObservableCollection<MyTask> GetTodayTask();
        ObservableCollection<MyTask> GetRepeatingTasks();
        MyTask GetTaskById(int id);


        // Timer ----------------------------------------------
        void SaveSession(Session session);
        List<Session> GetAllSessions();
        List<Session> GetAllSessionsWithTag(string tag);


        // Background -----------------------------------------
        public void SaveSelectedBackground(LinearGradientBrush selectedBrush);
        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2);

    }
}

