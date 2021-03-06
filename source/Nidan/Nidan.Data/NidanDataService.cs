﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using HR.Entity;
using Nidan.Entity;
using Nidan.Data.Extensions;
using Nidan.Data.Interfaces;
using Nidan.Entity.Dto;

namespace Nidan.Data
{
    public partial class NidanDataService : INidanDataService
    {
        private INidanDatabaseFactory _databaseFactory;
        private TransactionScope ReadUncommitedTransactionScope => new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted });

        public NidanDataService(INidanDatabaseFactory factory)
        {
            _databaseFactory = factory;
        }

        #region // Create

        public AbsenceType CreateAbsenceType(int organisationId, AbsenceType absenceType)
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                absenceType = context.AbsenceTypes.Add(absenceType);
                context.SaveChanges();

                return absenceType;
            }
        }

        public Enquiry CreateEnquiry(int organisationId, Enquiry enquiry)
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                enquiry = context.Enquiries.Add(enquiry);
                context.SaveChanges();

                return enquiry;
            }
        }
        
         public Mobilization CreateMobilization(int organisationId, Mobilization mobilization)
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                mobilization = context.Mobilizations.Add(mobilization);
                context.SaveChanges();

                return mobilization;
            }
        }


        public Personnel CreatePersonnel(int organisationId, Personnel personnel)
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                personnel = context.Personnels.Add(personnel);
                context.SaveChanges();

                return personnel;
            }

        }
    
        public T Create<T>(int organisationId, T t) where T : class
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                t = context.Set<T>().Add(t);
                context.SaveChanges();
                return t;
            }
        }

        public void Create<T>(int organisationId, IEnumerable<T> t) where T : class
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                context.Set<T>().AddRange(t);
                context.SaveChanges();
            }
        }
        #endregion

        #region // Retrieve

        public Event RetrieveEvent(int organisationId, int eventId, Expression<Func<Event, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Events
                    //.Include(c => c.)
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.EventId == eventId);
            }
        }

        public PagedResult<Event> RetrieveEvents(int organisationId, Expression<Func<Event, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
           
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Events
                    //.Include(c => c.EventBudgets)
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy> {
                        new OrderBy { Property = "Name", Direction = System.ComponentModel.ListSortDirection.Ascending }
                    })
                    .Paginate(paging);

            }
        }

        public AbsenceType RetrieveAbsenceType(int organisationId, int absenceTypeId, Expression<Func<AbsenceType, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .AbsenceTypes
                    .Include(c => c.Colour)
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.AbsenceTypeId == absenceTypeId);
            }
        }

        public PagedResult<AbsenceType> RetrieveAbsenceTypes(int organisationId, Expression<Func<AbsenceType, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .AbsenceTypes
                    .AsNoTracking()
                    .Include(c => c.Colour)
                    //.Include(a => a.CountryAbsenceTypes)
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy> {
                        new OrderBy { Property = "Name", Direction = System.ComponentModel.ListSortDirection.Ascending }
                    })
                    .Paginate(paging);
            }
        }

        public IEnumerable<Colour> RetrieveColours(Expression<Func<Colour, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create())
            {
                return context.Colours
                    .AsNoTracking()
                    .Where(predicate)
                    .ToList();
            }
        }

        public bool PersonnelEmploymentHasAbsences(int organisationId, int personnelId, int employmentId)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return true;
            }
        }

        public IEnumerable<Host> RetrieveHosts()
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create())
            {
                return context
                    .Hosts
                    .Include(o => o.Organisation)
                    .AsNoTracking()
                    .ToList();
            }
        }

        public IEnumerable<Organisation> RetrieveOrganisations()
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create())
            {
                return context
                    .Organisations
                    .Include(o => o.Hosts)
                    .AsNoTracking()
                    .ToList();
            }
        }

        public Personnel RetrievePersonnel(int organisationId, int personnelId, Expression<Func<Personnel, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Personnels
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.PersonnelId == personnelId);

            }
        }

        public IEnumerable<Personnel> RetrievePersonnel(int organisationId, IEnumerable<int> companyIds, IEnumerable<int> departmentIds, IEnumerable<int> divisionIds)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                var personnel = context
                    .Personnels
                    //.Include(p => p.Employments.Select(e => e.Division))
                    .AsNoTracking();

                //if (companyIds != null && companyIds.Any())
                //    personnel = personnel.Where(p => companyIds.Contains(p.Employments.OrderByDescending(by => by.StartDate).FirstOrDefault().Division.CompanyId));

                //if (departmentIds != null && departmentIds.Any())
                //    personnel = personnel.Where(p => departmentIds.Contains(p.Employments.OrderByDescending(by => by.StartDate).FirstOrDefault().DepartmentId));

                //if (divisionIds != null && divisionIds.Any())
                //    personnel = personnel.Where(p => divisionIds.Contains(p.Employments.OrderByDescending(by => by.StartDate).FirstOrDefault().DivisionId));

                return personnel.ToList();
            }
        }

        public PagedResult<Personnel> RetrievePersonnel(int organisationId, Expression<Func<Personnel, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {

                return context
                    .Personnels
                    .Include(p => p.Organisation)
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy>
                    {
                        new OrderBy
                        {
                            Property = "Forenames",
                            Direction = System.ComponentModel.ListSortDirection.Ascending
                        }
                    })
                    .Paginate(paging);
            }
        }

        public PagedResult<Question> RetrieveQuestions(int organisationId, Expression<Func<Question, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {

                return context
                    .Questions
                    .Include(p => p.Organisation)
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy>
                    {
                        new OrderBy
                        {
                            Property = "Description",
                            Direction = System.ComponentModel.ListSortDirection.Ascending
                        }
                    })
                    .Paginate(paging);
            }
        }

        public Question RetrieveQuestion(int organisationId, int questionId, Expression<Func<Question, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Questions
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.QuestionId == questionId);
            }
        }

        public UserAuthorisationFilter RetrieveUserAuthorisation(string aspNetUserId)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create())
            {
                return context
                    .UserAuthorisationFilters
                    .AsNoTracking()
                    .FirstOrDefault(u => u.AspNetUsersId == aspNetUserId);
            }
        }

        public PagedResult<Mobilization> RetrieveMobilizations(int organisationId, Expression<Func<Mobilization, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {

                return context
                    .Mobilizations
                    .Include(p => p.Organisation)
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy>
                    {
                        new OrderBy
                        {
                            Property = "Forenames",
                            Direction = System.ComponentModel.ListSortDirection.Ascending
                        }
                    })
                    .Paginate(paging);
            }
        }

        public PagedResult<PersonnelSearchField> RetrievePersonnelBySearchKeyword(int organisationId, string searchKeyword, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                var category = new SqlParameter("@SearchKeyword", searchKeyword);

                return context.Database
                    .SqlQuery<PersonnelSearchField>("SearchPersonnel @SearchKeyword", category).ToList().AsQueryable().
                    OrderBy(orderBy ?? new List<OrderBy>
                    {
                        new OrderBy
                        {
                            Property = "Forenames",
                            Direction = System.ComponentModel.ListSortDirection.Ascending
                        }
                    })
                    .Paginate(paging);
            }
        }

        //public PagedResult<EnquirySearchField> RetrieveEnquiryBySearchKeyword(int organisationId, string searchKeyword, List<OrderBy> orderBy, Paging paging)
        //{
        //    using (ReadUncommitedTransactionScope)
        //    using (var context = _databaseFactory.Create(organisationId))
        //    {
        //        var category = new SqlParameter("@SearchKeyword", searchKeyword);

        //        return context.Database
        //            .SqlQuery<EnquirySearchField>("SearchEnquiry @SearchKeyword", category).ToList().AsQueryable().
        //            OrderBy(orderBy ?? new List<OrderBy>
        //            {
        //                new OrderBy
        //                {
        //                    Property = "CandidateName",
        //                    Direction = System.ComponentModel.ListSortDirection.Ascending
        //                }
        //            })
        //            .Paginate(paging);
        //    }
        //}

        public List<T> Retrieve<T>(int organisationId, Expression<Func<T, bool>> predicate) where T : class
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                var returnItems = context.Set<T>().Where(predicate).ToList();
                return returnItems;
            }
        }

        public PagedResult<Enquiry> RetrieveEnquiries(int organisationId, Expression<Func<Enquiry, bool>> predicate, List<OrderBy> orderBy = null, Paging paging = null)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {

                return context
                    .Enquiries
                    .Include(p => p.Organisation)
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(orderBy ?? new List<OrderBy>
                    {
                        new OrderBy
                        {
                            Property = "CandidateName",
                            Direction = System.ComponentModel.ListSortDirection.Ascending
                        }
                    })
                    .Paginate(paging);
            }
        }

        public Enquiry RetrieveEnquiry(int organisationId, int enquiryId, Expression<Func<Enquiry, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Enquiries
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.EnquiryId == enquiryId);

            }
        }


        public Mobilization RetrieveMobilization(int organisationId, int mobilizationId, Expression<Func<Mobilization, bool>> predicate)
        {
            using (ReadUncommitedTransactionScope)
            using (var context = _databaseFactory.Create(organisationId))
            {
                return context
                    .Mobilizations
                    .AsNoTracking()
                    .Where(predicate)
                    .SingleOrDefault(p => p.MobilizationId == mobilizationId);

            }
        }

        #endregion

        #region // Update

        public T UpdateEntityEntry<T>(T t) where T : class
        {
            using (var context = _databaseFactory.Create())
            {
                context.Entry(t).State = EntityState.Modified;
                context.SaveChanges();

                return t;
            }
        }

        public T UpdateOrganisationEntityEntry<T>(int organisationId, T t) where T : class
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                context.Entry(t).State = EntityState.Modified;
                context.SaveChanges();

                return t;
            }
        }

        #endregion

        #region //Delete
        public void Delete<T>(int organisationId, Expression<Func<T, bool>> predicate) where T : class
        {
            using (var context = _databaseFactory.Create(organisationId))
            {
                var items = context.Set<T>().Where(predicate).FirstOrDefault();
                context.Set<T>().Remove(items);
                context.SaveChanges();
            }
        }

        


        #endregion
    }
}
