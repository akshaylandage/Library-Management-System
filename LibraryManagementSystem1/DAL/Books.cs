using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using LibraryManagementSystem1.Models;

namespace LibraryManagementSystem1.DAL
{
    public class Books
    {
    }
}




public class Books
{
    #region Basic Functionality

    #region Variable Declaration


    private Database db;
    #endregion

    #region Constructors

    public Books()
    {
        this.db = DatabaseFactory.CreateDatabase();
    }
    public Books(int bookId)
    {
        this.db = DatabaseFactory.CreateDatabase();
        this.BookId = bookId;
    }
    #endregion

    #region Properties

    public int PageLength { get; set; } = 10;

    public int PageNumber { get; set; } = 1;

    public List<BooksModel> GetBooks { get; set; }

    public int BookId { get; set; }

    public string BookName { get; set; }

    public int BookCategoryId { get; set; }

    public int BookPublisherId { get; set; }

    public bool IsActive { get; set; }

    public int BookQuantity { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public int TotalCount { get; set; }
    #endregion

    #region Actions
    public bool Load()
    {
        try
        {
            if (this.BookId != 0)
            {
                DbCommand com = this.db.GetStoredProcCommand("BooksGetDetails");
                this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
                DataSet ds = this.db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    this.BookId = Convert.ToInt32(dt.Rows[0]["BookId"]);
                    this.BookName = Convert.ToString(dt.Rows[0]["BookName"]);

                    this.BookCategoryId = Convert.ToInt32(dt.Rows[0]["BookCategoryId"]);
                    this.BookPublisherId = Convert.ToInt32(dt.Rows[0]["BookPublisherId"]);

                    this.IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    this.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                    this.CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
                    this.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
                    this.ModifiedOn = Convert.ToDateTime(dt.Rows[0]["ModifiedOn"]);
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            // To Do: Handle Exception
            return false;
        }
    }

    /// <summary>
    /// Inserts details for Books if BookId = 0.
    /// Else updates details for Books.
    /// </summary>
    /// <returns>True if Save operation is successful; Else False.</returns>
    public bool Save()
    {
        if (this.BookId == 0)
        {
            return this.Insert();
        }
        else
        {
            if (this.BookId > 0)
            {
                return this.Update();
            }
            else
            {
                this.BookId = 0;
                return false;
            }
        }
    }

    /// <summary>
    /// Inserts details for Books.
    /// Saves newly created Id in BookId.
    /// </summary>
    /// <returns>True if Insert operation is successful; Else False.</returns>
    public bool Insert()
    {
        try
        {
            DbCommand com = this.db.GetStoredProcCommand("BooksInsert");
            this.db.AddOutParameter(com, "BookId", DbType.Int32, 14);
            if (!String.IsNullOrEmpty(this.BookName))
            {
                this.db.AddInParameter(com, "BookName", DbType.String, this.BookName);
            }
            else
            {
                this.db.AddInParameter(com, "BookName", DbType.String, DBNull.Value);
            }

            ///////////////////////////////////////////////////////////////////////////
            if (this.BookCategoryId > 0)
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, this.BookCategoryId);
            }
            else
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, DBNull.Value);
            }
            if (this.BookPublisherId > 0)
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, this.BookPublisherId);
            }
            else
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, DBNull.Value);
            }
            if (this.BookQuantity > 0)
            {
                this.db.AddInParameter(com, "BookQuantity", DbType.Int32, this.BookQuantity);
            }
            else
            {
                this.db.AddInParameter(com, "BookQuantity", DbType.Int32, DBNull.Value);
            }
            ///////////////////////////////////////////////////////////////////////////
            if (IsActive)
            {
                this.db.AddInParameter(com, "IsActive", DbType.Boolean, 1);
            }
            else
            {
                this.db.AddInParameter(com, "IsActive", DbType.Boolean, 0);
            }

            if (this.CreatedBy > 0)
            {
                this.db.AddInParameter(com, "CreatedBy", DbType.Int32, this.CreatedBy);
            }
            else
            {
                this.db.AddInParameter(com, "CreatedBy", DbType.Int32, 1);
            }
            /*if (this.CreatedOn > DateTime.MinValue)
            {
                this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, this.CreatedOn);
            }
            else
            {
                this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, DBNull.Value);
            }*/
            if (this.ModifiedBy > 0)
            {
                this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
            }
            else
            {
                this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, 1);
            }
            /*if (this.ModifiedOn > DateTime.MinValue)
            {
                this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, this.ModifiedOn);
            }
            else
            {
                this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, DBNull.Value);
            }*/
            this.db.ExecuteNonQuery(com);
            this.BookId = Convert.ToInt32(this.db.GetParameterValue(com, "BookId"));      // Read in the output parameter value
        }
        catch (Exception ex)
        {
            // To Do: Handle Exception
            return false;
        }

        return this.BookId > 0; // Return whether ID was returned
    }

    /// <summary>
    /// Updates details for Books.
    /// </summary>
    /// <returns>True if Update operation is successful; Else False.</returns>


    private bool Update()
    {
        try
        {
            DbCommand com = this.db.GetStoredProcCommand("BooksUpdate");
            this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
            if (!String.IsNullOrEmpty(this.BookName))
            {
                this.db.AddInParameter(com, "BookName", DbType.String, this.BookName);
            }
            else
            {
                this.db.AddInParameter(com, "BookName", DbType.String, DBNull.Value);
            }

            ///////////////////////////////////////////////////////////////////////////
            if (this.BookCategoryId > 0)
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, this.BookCategoryId);
            }
            else
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, DBNull.Value);
            }
            if (this.BookPublisherId > 0)
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, this.BookPublisherId);
            }
            else
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, DBNull.Value);
            }
            ///////////////////////////////////////////////////////////////////////////

            this.db.AddInParameter(com, "IsActive", DbType.Boolean, this.IsActive);
            if (this.CreatedBy > 0)
            {
                this.db.AddInParameter(com, "CreatedBy", DbType.Int32, this.CreatedBy);
            }
            else
            {
                this.db.AddInParameter(com, "CreatedBy", DbType.Int32, DBNull.Value);
            }
            if (this.CreatedOn > DateTime.MinValue)
            {
                this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, this.CreatedOn);
            }
            else
            {
                this.db.AddInParameter(com, "CreatedOn", DbType.DateTime, DBNull.Value);
            }
            if (this.ModifiedBy > 0)
            {
                this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
            }
            else
            {
                this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, DBNull.Value);
            }
            if (this.ModifiedOn > DateTime.MinValue)
            {
                this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, this.ModifiedOn);
            }
            else
            {
                this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, DBNull.Value);
            }
            this.db.ExecuteNonQuery(com);
        }
        catch (Exception ex)
        {
            // To Do: Handle Exception
            return false;
        }

        return true;
    }

    /// <summary>
    /// Deletes details of Books for provided BookId.
    /// </summary>
    /// <returns>True if Delete operation is successful; Else False.</returns>
    public bool Delete()
    {
        try
        {
            DbCommand com = this.db.GetStoredProcCommand("BooksDelete");
            this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
            this.db.ExecuteNonQuery(com);
        }
        catch (Exception ex)
        {
            // To Do: Handle Exception
            return false;
        }

        return true;
    }

    /// <summary>
    /// Get list of Books for provided parameters.
    /// </summary>
    /// <returns>DataSet of result</returns>
    /// <remarks></remarks>

    public List<BooksModel> GetList()
    {
        DataSet dsBooksList = null;
        List<BooksModel> BookList = null;
        try
        {

            DbCommand com = this.db.GetStoredProcCommand("BooksGetList");

            if (!String.IsNullOrEmpty(this.BookName))
            {
                this.db.AddInParameter(com, "BookName", DbType.String, this.BookName);
            }
            else
            {
                this.db.AddInParameter(com, "BookName", DbType.String, DBNull.Value);
            }
            if (this.BookCategoryId > 0)
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, this.BookCategoryId);
            }
            else
            {
                this.db.AddInParameter(com, "BookCategoryId", DbType.Int32, DBNull.Value);
            }
            if (this.BookPublisherId > 0)
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, this.BookPublisherId);
            }
            else
            {
                this.db.AddInParameter(com, "BookPublisherId", DbType.Int32, DBNull.Value);
            }
            if (this.PageLength > 0)
            {
                this.db.AddInParameter(com, "PageLength", DbType.Int32, this.PageLength);
            }
            else
            {
                this.db.AddInParameter(com, "PageLength", DbType.Int32, 10);
            }
            if (this.PageNumber > 0)
            {
                this.db.AddInParameter(com, "PageNumber", DbType.Int32, this.PageNumber);
            }
            else
            {
                this.db.AddInParameter(com, "PageNumber", DbType.Int32, 1);
            }
            this.db.ExecuteNonQuery(com);
            dsBooksList = db.ExecuteDataSet(com);

            BookList = new List<BooksModel>();

            for (int i = 0; i < dsBooksList.Tables[0].Rows.Count; i++)
            {
                BookList.Add(new BooksModel()
                {
                    BookId = Convert.ToInt32(dsBooksList.Tables[0].Rows[i]["BookId"]),
                    BookName = dsBooksList.Tables[0].Rows[i]["BookName"].ToString(),
                    BookCategoryName = dsBooksList.Tables[0].Rows[i]["BookCategoryName"].ToString(),
                    BookPublisherName = dsBooksList.Tables[0].Rows[i]["BookPublisherName"].ToString(),
                    BookQuantity = Convert.ToInt32(dsBooksList.Tables[0].Rows[i]["BookQuantity"]),

                });
            }

            TotalCount = Convert.ToInt32(dsBooksList.Tables[1].Rows[0]["TotalCount"]);
        }
        catch (Exception ex)
        {
            //To Do: Handle Exception
        }

        return BookList;
    }

    public List<BooksModel> GetCategoryList()
    {
        DataSet dsBookCategoryList = null;
        List<BooksModel> BookCategoryList = null;
        try
        {
            DbCommand com = this.db.GetStoredProcCommand("BooksCategoryList");
            dsBookCategoryList = db.ExecuteDataSet(com);
            BookCategoryList = new List<BooksModel>();

            for (int i = 0; i < dsBookCategoryList.Tables[0].Rows.Count; i++)
            {
                BookCategoryList.Add(new BooksModel()
                {
                    BookCategoryId = Convert.ToInt32(dsBookCategoryList.Tables[0].Rows[i]["BookCategoryId"]),
                    BookCategory = dsBookCategoryList.Tables[0].Rows[i]["BookCategory"].ToString(),
                });
            }
        }
        catch (Exception ex)
        {
            //To Do: Handle Exception
        }

        return BookCategoryList;
    }

    public List<BooksModel> GetPublisherList()
    {
        DataSet dsBookPublisherList = null;
        List<BooksModel> BookPublisherList = null;
        try
        {
            DbCommand com = this.db.GetStoredProcCommand("BooksPublisherList");
            dsBookPublisherList = db.ExecuteDataSet(com);
            BookPublisherList = new List<BooksModel>();

            for (int i = 0; i < dsBookPublisherList.Tables[0].Rows.Count; i++)
            {
                BookPublisherList.Add(new BooksModel()
                {
                    BookPublisherId = Convert.ToInt32(dsBookPublisherList.Tables[0].Rows[i]["BookPublisherId"]),
                    BookPublisher = dsBookPublisherList.Tables[0].Rows[i]["BookPublisher"].ToString(),
                });
            }
        }
        catch (Exception ex)
        {
            //To Do: Handle Exception
        }

        return BookPublisherList;
    }
    #endregion

    #endregion
}

