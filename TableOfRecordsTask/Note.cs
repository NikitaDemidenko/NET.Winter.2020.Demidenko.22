using System;
using System.Collections.Generic;
using System.Text;

namespace TableOfRecordsTask
{
    /// <summary>Note.</summary>
    public class Note
    {
        /// <summary>Initializes a new instance of the <see cref="Note"/> class.</summary>
        /// <param name="content">Content of note.</param>
        /// <param name="title">Title of note.</param>
        /// <exception cref="ArgumentNullException">Thrown when content is null.</exception>
        public Note(string title, string content)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            this.Title = title;
            this.Content = content;
            this.TimeOfCreation = DateTime.Now;
        }

        /// <summary>Gets or sets the title.</summary>
        /// <value>Title of note.</value>
        public string Title { get; set; }

        /// <summary>Gets or sets the content.</summary>
        /// <value>Content of note.</value>
        public string Content { get; set; }

        /// <summary>Gets or sets the time of creation.</summary>
        /// <value>Note creation time.</value>
        public DateTime TimeOfCreation { get; set; }
    }
}
