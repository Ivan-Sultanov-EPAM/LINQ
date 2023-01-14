using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers
                .Where(x => x.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers
                .Select(c =>
                    (c, suppliers.Where(s => s.Country == c.Country && s.City == c.City))
                );
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return from c in customers
                   join s in suppliers on c.Country equals s.Country into g
                   select (c, g.Where(s => s.City == c.City));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers
                .Where(c => c.Orders.Any(o => o.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (c, c.Orders.OrderBy(o => o.OrderDate).First().OrderDate));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (c, c.Orders.OrderBy(o => o.OrderDate).First().OrderDate))
                .OrderBy(x => x.OrderDate.Year)
                .ThenBy(x => x.OrderDate.Month)
                .ThenByDescending(x => x.c.Orders.First(o => o.OrderDate == x.OrderDate).Total)
                .ThenBy(x => x.c.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers
                .Where(c =>
                c.PostalCode.Any(ch => !char.IsDigit(ch)) ||
                c.Region == null ||
                c.Phone.All(ch => ch != '(' && ch != ')')
                );
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            return products
                .GroupBy(p => p.Category)
                .Select(g => new Linq7CategoryGroup
                {
                    Category = g.Key,
                    UnitsInStockGroup = g.AsEnumerable()
                        .GroupBy(p => p.UnitsInStock)
                        .Select(gr => new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = gr.Key,
                            Prices = gr
                                .AsEnumerable()
                                .Select(p => p.UnitPrice)
                                .OrderBy(p => p)
                        })
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            return products
                .GroupBy(p =>
                {
                    if (p.UnitPrice >= 0 && p.UnitPrice <= cheap)
                    {
                        return cheap;
                    }

                    if (p.UnitPrice > cheap && p.UnitPrice <= middle)
                    {
                        return middle;
                    }

                    if (p.UnitPrice > middle && p.UnitPrice <= expensive)
                    {
                        return expensive;
                    }

                    throw new ArgumentOutOfRangeException();
                })
                .Select(g => (g.Key, g.AsEnumerable()));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            return customers
                .GroupBy(
                    c => c.City,
                    c => new
                    {
                        Count = c.Orders.Length,
                        OrdersTotal = c.Orders.Sum(o => o.Total)
                    }
                )
                .Select(g => (
                    g.Key,
                    (int)Math.Round(g.Average(c => c.OrdersTotal)),
                    (int)Math.Round(g.Average(c => c.Count))
                    ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return suppliers
                    .Select(s => s.Country)
                    .Distinct()
                    .OrderBy(c => c.Length)
                    .ThenBy(c => c)
                    .Aggregate((a, b) => a + b);
        }
    }
}