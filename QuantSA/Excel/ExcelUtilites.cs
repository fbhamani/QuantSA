﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantSA.Excel
{

    /// <summary>
    /// Functions that will be used in many places when gettting data ready to send to and from Excel
    /// </summary>
    public class ExcelUtilites
    {

        /// <summary>
        /// Converts a double reflecting an Excel date into a <see cref="Date"/>.
        /// </summary>
        /// <param name="excelDate">A value reflecting an Excel date.</param>        
        /// <returns></returns>
        public static Date GetDates(double excelDate)
        {
            return new Date(DateTime.FromOADate(excelDate));            
        }

        /// <summary>
        /// Converts doubles reflecting Excel dates into <see cref="Date"/>s
        /// </summary>
        /// <param name="excelDates">Vector of values reflecting Excel dates.</param>
        /// <returns></returns>
        public static Date[] GetDates(double[] excelDates)
        {
            Date[] result = new Date[excelDates.Length];
            for (int i=0; i<excelDates.Length; i++)
            {
                result[i] = new Date(DateTime.FromOADate(excelDates[i]));
            }
            //return excelDates.Select(date => new Date(DateTime.FromOADate(date))).ToArray();
            //TODO: RJ: loop or select
            return result;
        }

        /// <summary>
        /// Converts doubles reflecting Excel dates into <see cref="Date"/>s
        /// </summary>
        /// <param name="excelDates">2d array of Excel values.</param>
        /// <returns></returns>
        public static Date[,] GetDates(double[,] excelDates)
        {
            Date[,] result = new Date[excelDates.GetLength(0), excelDates.GetLength(1)];
            for (int i = 0; i < excelDates.GetLength(0); i++)
            {
                for (int j = 0; j < excelDates.GetLength(1); j++)
                {
                    result[i, j] = new Date(DateTime.FromOADate(excelDates[i, j]));
                }
            }
            return result;
        }

        /// <summary>
        /// Convert an erorr message to an object for return to Excel.
        /// </summary>
        /// <param name="e">The exception that has been thrown and needs to be displaced in the cell.</param>
        /// <returns></returns>
        public static object Error0D(Exception e)
        {
            return e.Message;
        }

        /// <summary>
        /// Convert an erorr message to a 1d object array for return to Excel.
        /// </summary>
        /// <param name="e">The exception that has been thrown and needs to be displaced in the cell.</param>
        /// <returns></returns>
        public static object[] Error1D(Exception e)
        {
            return new object[] { e.Message };
        }

        /// <summary>
        /// Convert an erorr message to a 2d object array for return to Excel.
        /// </summary>
        /// <param name="e">The exception that has been thrown and needs to be displaced in the cell.</param>
        /// <returns></returns>
        public static object[,] Error2D(Exception e)
        {
            return new object[,] { { e.Message } };
        }


        /// <summary>
        /// Convert a double array of doubles into objects for returning to excel. 
        /// </summary>
        /// <remarks>
        /// Note that we use objects in Excel return types so that we can send values or strings to the cell. 
        /// </remarks>
        /// <param name="result"></param>
        /// <returns></returns>
        public static object[,] GetObjects(double[,] result)
        {
            object[,] resultObj = new object[result.GetLength(0), result.GetLength(1)];
            for (int i = 0; i<result.GetLength(0); i++ )
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    resultObj[i, j] = result[i, j];
                }
            }
            return resultObj;
        }
    }
}