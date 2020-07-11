﻿namespace VidyaSadhan_API.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string QuestionDetail { get; set; }

        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }

        public bool IsOption { get; set; }

        public string Answer { get; set; }
    }
}