using AutoMapper.Configuration.Conventions;
using webAPI2.Models;

namespace webAPI2.Interface
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId); 
        bool ReviewerExists(int reviewerId);
        bool CreateReviewer (Reviewer reviewer);
        bool Save();
    }
}
