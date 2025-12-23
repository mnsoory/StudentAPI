using StudentAPI.Domain.Entities;

namespace StudentAPI.Domain.Interfaces
{
    // The interface defines the "contract" between the Application layer and the Infrastructure layer.
    public interface IStudentRepository
    {
        //// ===============================================
        //// C - Create
        //// ===============================================

        /// <summary>
        /// Adds a new student entity to the persistence store.
        /// </summary>
        /// <param name="student">The student entity.</param>
        /// <returns>The added Student entity (may include a populated ID).</returns>
        Task AddAsync(Student student);


        //// ===============================================
        //// R - Read / Query
        //// ===============================================

        /// <summary>
        /// Retrieves a single student using their unique identifier.
        /// </summary>
        /// <param name="id">The student identifier (INT).</param>
        /// <returns>The Student entity if found, or null.</returns>
        Task<Student?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a list of all students.
        /// </summary>
        /// <returns>A collection of Student entities.</returns>
        Task<IEnumerable<Student>> GetAllAsync();

        /// <summary>
        /// Retrieves a list of all passed students.
        /// </summary>
        /// <returns>A collection of Student entities.</returns>
        Task<IEnumerable<Student>> GetPassedStudentsAsync();

        /// <summary>
        /// Calculate the Average grade of students grades.
        /// </summary>
        /// <returns>A double of Students average grade.</returns>
        Task<double?> GetAverageGrade();


        //// ===============================================
        //// U - Update
        //// ===============================================

        /// <summary>
        /// Updates an existing student entity in the persistence store.
        /// </summary>
        /// <param name="student">The updated student entity.</param>
        /// <returns>A Task representing the asynchronous operation (void equivalent).</returns>
        Task UpdateAsync(Student student);


        // ===============================================
        // D - Delete
        // ===============================================

        /// <summary>
        /// Deletes a student based on their identifier.
        /// </summary>
        /// <param name="student">The student to be deleted.</param>
        /// <returns>A Task representing the asynchronous operation (void equivalent).</returns>
        Task DeleteAsync(Student student);

    }
}