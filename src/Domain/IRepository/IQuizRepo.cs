﻿using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IQuizRepo
    {
        Task<Quiz> GetQuizById(Guid id);
        Task<IEnumerable<Quiz>> GetQuizByLessonId(Guid lessonId);
        Task<bool> CreateQuiz(Quiz quiz);
        Task<bool> UpdateQuiz(Quiz quiz);
        Task<bool> DeleteQuiz(Guid id);
    }
}
