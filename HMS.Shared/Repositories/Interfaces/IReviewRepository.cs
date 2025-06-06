using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Review DTOs in the data store.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Gets all reviews asynchronously.
        /// </summary>
        /// <returns>A collection of all review DTOs.</returns>
        Task<IEnumerable<ReviewDto>> GetAllAsync();

        /// <summary>
        /// Gets a review by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The review DTO if found; otherwise null.</returns>
        Task<ReviewDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new review asynchronously.
        /// </summary>
        /// <param name="review">The review DTO to add.</param>
        /// <returns>The added review DTO.</returns>
        Task<ReviewDto> AddAsync(ReviewDto review);

        /// <summary>
        /// Updates an existing review asynchronously.
        /// </summary>
        /// <param name="review">The review DTO with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(ReviewDto review);

        /// <summary>
        /// Deletes a review by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
