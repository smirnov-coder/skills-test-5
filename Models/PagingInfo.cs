namespace SkillsTest.Models
{
    /// <summary>
    /// Информация о пагинации.
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// Количество объектов на одной странице.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Номер текущей страницы.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Общее количество объектов.
        /// </summary>
        public int ItemsTotalCount { get; set; }
    }
}
