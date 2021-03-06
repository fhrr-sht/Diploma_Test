﻿using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserInteraction
{
    public class UserOutput
    {
        public static void TableExpenses(int from, int to)
        {

            List<Expenses> expensesList = Data.GetExpenses().Where(x => x.Id > from && x.Id <= to).ToList();
            int maxEnum = Enum.GetValues(typeof(CatalogType)).Cast<int>().Max();
            int minEnum = Enum.GetValues(typeof(CatalogType)).Cast<int>().Min();
            for (CatalogType catalogType = (CatalogType)minEnum; catalogType <= (CatalogType)maxEnum; catalogType++)
            {
                Data.CreateFile(catalogType.ToString());
            }

            List<Catalog> categories = Data.GetList(CatalogType.GoodsCategory + ".csv");
            List<Catalog> goods = Data.GetList(CatalogType.Goods + ".csv");
            List<Catalog> units = Data.GetList(CatalogType.Unit + ".csv");


            int[] maxWidth = new int[] {Constant.IdColumnLength,
                    Constant.CategoryColumnLength,
                    Constant.NameColumnLength,
                    Constant.UnitColumnLength,
                    Constant.PriceColumnLength,
                    Constant.QuantityColumnLength,
                    Constant.DateColumnLength};
            Console.Clear();
            Console.WriteLine(" .____________________________________________________________________________________________________________________.");
            Console.WriteLine(" |                                                                                                                    |");
            Console.WriteLine(" |                                                 ПОКУПКИ                                                            |");
            Console.WriteLine(" |____________________________________________________________________________________________________________________|");
            Console.WriteLine(" |     |                     |                               |                    |            |   Кол-  |            |");
            Console.WriteLine(" | ID  |      Категория      |         Наименование          |     Ед. измер.     |    Цена    |   ство  |    Дата    |");
            Console.WriteLine(" |_____|_____________________|_______________________________|____________________|____________|_________|____________|");
            for (int i = 0; i < expensesList.Count; i++)
            {
                Console.WriteLine(" |     |                     |                               |                    |            |         |            |");
                string[] row = new string[] {expensesList[i].Id.ToString(),
                    categories.FirstOrDefault(x => x.Id == expensesList[i].CategoryId).Name,
                    goods.FirstOrDefault(x => x.Id == expensesList[i].GoodsId).Name,
                    units.FirstOrDefault(x => x.Id == expensesList[i].UnitId).Name,
                    expensesList[i].Price.ToString(),
                    expensesList[i].Quantity.ToString(),
                    expensesList[i].Date.ToShortDateString()};
                CellLineBreak(row, maxWidth);
                Console.WriteLine(" |_____|_____________________|_______________________________|____________________|____________|_________|____________|");
            }
        }
        public static void TableCatalogs(CatalogType catalogType, int from, int to)
        {
            string filePath = Data.CreateFile(catalogType.ToString());
            List<Catalog> catalog = Data.GetList(filePath).Where(x => x.Id > from && x.Id <= to).ToList();
            int[] maxWidth = new int[] { Constant.IdCatColumnLength, Constant.NameCatColumnLength };

            Console.Clear();
            Console.WriteLine(" .____________________________________________________________________________________________________________________.");
            Console.WriteLine(" |                                                                                                                    |");
            Console.WriteLine(" |                  {0}  |", catalogType + new string(' ', 96 - catalogType.ToString().Length));
            Console.WriteLine(" |____________________________________________________________________________________________________________________|");
            Console.WriteLine(" |       |                                                                                                            |");
            Console.WriteLine(" | ID    |     Наименование                                                                                           |");
            Console.WriteLine(" |_______|____________________________________________________________________________________________________________|");
            for (int i = 0; i < catalog.Count; i++)
            {
                Console.WriteLine(" |       |                                                                                                            |");
                string[] row = new string[] { catalog[i].Id.ToString(), catalog[i].Name };
                CellLineBreak(row, maxWidth);

                Console.WriteLine(" |_______|____________________________________________________________________________________________________________|");
            }
        }
        private static void CellLineBreak(string[] row, int[] maxWidth)
        {
            // Count of extra split row.
            int splitRow = 0;
            // If any cell data is more than max width, then it will need extra row.
            bool needExtraRow;
            do
            {
                needExtraRow = false;
                String[] newRow = new String[row.Length];
                for (int i = 0; i < row.Length; i++)
                {
                    // If data is less than max width, use that as it is.
                    if (row[i].Length < maxWidth[i])
                    {
                        newRow[i] = splitRow == 0 ? row[i] : "";
                    }
                    else if ((row[i].Length > (splitRow * maxWidth[i])))
                    {
                        // If data is more than max width, then crop data at maxwidth.
                        // Remaining cropped data will be part of next row.
                        int end = row[i].Length > ((splitRow * maxWidth[i]) + maxWidth[i])
                               ? maxWidth[i]
                               ://( (row[i].Length- ((splitRow  maxWidth) + maxWidth))<0 ? ((splitRow  maxWidth) + maxWidth) - row[i].Length : row[i].Length - ((splitRow * maxWidth) + maxWidth));
                               row[i].Length - (splitRow * maxWidth[i]);
                        newRow[i] = row[i].Substring((splitRow * maxWidth[i]), end);
                        needExtraRow = true;
                    }
                    else
                    {
                        newRow[i] = "";
                    }
                }
                for (int j = 0; j < newRow.Length; j++)
                {
                    string item = (j == 0 ? " | " : "| ") + newRow[j] + new string(' ', (j < newRow.Length - 1 ? maxWidth[j] - newRow[j].Length : maxWidth[j] - newRow[j].Length - 1));
                    Console.Write(item.ToString());
                }
                Console.Write("| ");

                if (needExtraRow)
                {
                    splitRow++;
                    Console.WriteLine();
                }
            } while (needExtraRow);
            Console.WriteLine();
        }
        public static void TableExpensesDynamic(int from, int to)
        {
            List<Expenses> expensesList = Data.GetExpenses().Where(x => x.Id > from && x.Id <= to).ToList();
            int maxEnum = Enum.GetValues(typeof(CatalogType)).Cast<int>().Max();
            int minEnum = Enum.GetValues(typeof(CatalogType)).Cast<int>().Min();
            for (CatalogType catalogType = (CatalogType)minEnum; catalogType <= (CatalogType)maxEnum; catalogType++)
            {
                Data.CreateFile(catalogType.ToString());
            }

            List<Catalog> categories = Data.GetList(CatalogType.GoodsCategory + ".csv");
            List<Catalog> goods = Data.GetList(CatalogType.Goods + ".csv");
            List<Catalog> units = Data.GetList(CatalogType.Unit + ".csv");
            string mainHeader = "ПОКУПКИ";
            string[] header = new string[] { "ID", "Категория", "Наименование", "Ед.измер.", "Цена", "Кол-во", "Дата" };
            int WindowWidth = Console.WindowWidth - 4;
            int[] maxWidth = new int[] {
                    Constant.IdColumnPers,
                    Constant.CategoryColumnPers,
                    Constant.NameColumnPers,
                    Constant.UnitColumnPers,
                    Constant.PriceColumnPers,
                    Constant.QuantityColumnPers,
                    Constant.DateColumnPers};
            for (int i = 1; i < maxWidth.Length; i++)
            {
                maxWidth[i] = (int)(Console.WindowWidth / 100f * maxWidth[i]);
            }
            Console.Clear();
            Console.WriteLine(" ." + new string('_', WindowWidth) + ".");
            Console.WriteLine(" |" + new string(' ', WindowWidth) + "|");
            Console.WriteLine(" |" + WriteCenter(mainHeader, WindowWidth));
            Console.WriteLine(" |" + new string('_', WindowWidth) + "|");
            Console.WriteLine(" |" + WriteStr(maxWidth, ' '));
            Console.WriteLine(" |" + WriteCenterStr(header, maxWidth));
            Console.WriteLine(" |" + WriteStr(maxWidth, '_'));
            for (int i = 0; i < expensesList.Count; i++)
            {
                Console.WriteLine(" |" + WriteStr(maxWidth, ' '));
                string[] row = new string[] {expensesList[i].Id.ToString(),
                    categories.FirstOrDefault(x => x.Id == expensesList[i].CategoryId).Name,
                    goods.FirstOrDefault(x => x.Id == expensesList[i].GoodsId).Name,
                    units.FirstOrDefault(x => x.Id == expensesList[i].UnitId).Name,
                    expensesList[i].Price.ToString(),
                    expensesList[i].Quantity.ToString(),
                    expensesList[i].Date.ToShortDateString()};
                CellLineBreak(row, maxWidth);
                Console.WriteLine(" |" + WriteStr(maxWidth, '_'));
            }
        }

        private static string WriteStr(int[] maxWidth, char fill)
        {
            string line = String.Empty;
            for (int i = 0; i < maxWidth.Length; i++)
            {
                line = line + (new string(fill, maxWidth[i] - 1) + "|");
            }
            return line;
        }
        private static string WriteCenterStr(string[] word, int[] maxWidth)
        {
            string line = String.Empty;
            for (int i = 0; i < maxWidth.Length; i++)
            {
                line = line + WriteCenter(word[i], maxWidth[i]-1);
            }
            return line;
        }

        private static string WriteCenter(string word, int maxWidth)
        {
            return new string(' ', (maxWidth) / 2 - word.Length / 2) + $"{word}" + new string(' ', (maxWidth) / 2 - word.Length / 2) + "|";
        }
    }
}