using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.DTOs.EventDTOs;
using MyAPI.Models;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml.Linq;

namespace MyAPI.DAO
{
    public class EventDAO
    {
        private readonly EventManagementContext _context;
        private readonly IMapper _mapper;
        public EventDAO(EventManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task addEvent(EventDTO events)
        {
            var eventMap = _mapper.Map<Event>(events);
             _context.Events.Add(eventMap);
             _context.SaveChanges();
           
        }

        public List<EventDTO> getListEvent(int userId)
        {
            var listEvent = from ue in _context.UserEvents
                            join e in _context.Events on ue.EventId equals e.EventId
                            join u in _context.Users on ue.UserId equals u.UserId
                            join c in _context.EventCategories on e.CategoryId equals c.CategoryId
                            where u.UserId == userId 
                            select new EventDTO
                            {
                                EventId = e.EventId,
                                EventName = e.EventName,
                                CategoryName = c.CategoryName,
                                EventDate = e.EventDate,
                                EventDescription = e.EventDescription,
                                Location = e.Location,
                                NumberPerson = e.NumberPerson,
                                Status  = e.Status,
                                Image = e.Image
                            };

            return listEvent.ToList(); 
        }

        public bool checkEventExist(int id)
        {
            var check = _context.Events.FirstOrDefault(x => x.EventId == id);
            if (check == null) 
            { 
                return false;
            }
            return true;
        }
        public Event GetEvent(int id) 
        {
            var eventById = _context.Events.FirstOrDefault(x => x.EventId == id);
            return eventById;
        }
        public void updateEventById(int id, EventDTO events)
        {
            var eventById = GetEvent(id);
            if (eventById != null) 
            {
                eventById.CategoryId = events.CategoryId;
                eventById.EventDate = events.EventDate;
                eventById.EventDescription = events.EventDescription;
                eventById.EventName = events.EventName;
                eventById.Location = events.Location;
                eventById.NumberPerson = events.NumberPerson;
                eventById.Image = events.Image;
                eventById.Status = events.Status;
                _context.Events.Update(eventById);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Event with ID {id} not found.");
            }
        }

        public void delete(Event events)
        {
            var userEvent = _context.UserEvents.FirstOrDefault(x => x.EventId.Equals(events.EventId));
            _context.UserEvents.Remove(userEvent);
            _context.SaveChanges();
            _context.Events.Remove(events);
            _context.SaveChanges();
        }

		public EventDTO getEventById(int id)
		 {
			var events = _context.Events
	   .Include(x => x.Category)
	   .FirstOrDefault(x => x.EventId == id);

			if (events == null)
			{
				// Xử lý khi không tìm thấy sự kiện
				return null;
			}

			var eventDTO = _mapper.Map<EventDTO>(events);
			return eventDTO;
		}
        public int getLastID()
        {
            var lastID = _context.Events.OrderByDescending(x => x.EventId).FirstOrDefault();
            return lastID.EventId;
        }
        public List<EventDTO> getEventByName(string name, int userId)
        {
            var listEvent = from ue in _context.UserEvents
                            join e in _context.Events on ue.EventId equals e.EventId
                            join u in _context.Users on ue.UserId equals u.UserId
                            join c in _context.EventCategories on e.CategoryId equals c.CategoryId
                            where u.UserId == userId && e.EventName.Contains(name)
                            select new EventDTO
                            {
                                EventId = e.EventId,
                                EventName = e.EventName,
                                CategoryName = c.CategoryName,
                                EventDate = e.EventDate,
                                EventDescription = e.EventDescription,
                                Location = e.Location,
                                NumberPerson = e.NumberPerson,
                                Status = e.Status,
                                Image = e.Image
                            };

            return listEvent.ToList();
        }
        public List<EventDTO> getEventByDate( int userId)
        {
            var listEvent = from ue in _context.UserEvents
                            join e in _context.Events on ue.EventId equals e.EventId
                            join u in _context.Users on ue.UserId equals u.UserId
                            join c in _context.EventCategories on e.CategoryId equals c.CategoryId
                            where u.UserId == userId && e.EventDate >= DateTime.Now
                            select new EventDTO
                            {
                                EventId = e.EventId,
                                EventName = e.EventName,
                                CategoryName = c.CategoryName,
                                EventDate = e.EventDate,
                                EventDescription = e.EventDescription,
                                Location = e.Location,
                                NumberPerson = e.NumberPerson,
                                Status = e.Status,
                                Image = e.Image
                            };
            return listEvent.ToList();
        }
        public List<EventDTO> getHistoryEvent(int userId,string name)
        {
            var listEvent = from ue in _context.UserEvents
                            join e in _context.Events on ue.EventId equals e.EventId
                            join u in _context.Users on ue.UserId equals u.UserId
                            join c in _context.EventCategories on e.CategoryId equals c.CategoryId
                            where u.UserId == userId && e.EventDate <= DateTime.Now && e.Status == "Completed" 
                            select new EventDTO
                            {
                                EventId = e.EventId,
                                EventName = e.EventName,
                                CategoryName = c.CategoryName,
                                EventDate = e.EventDate,
                                EventDescription = e.EventDescription,
                                Location = e.Location,
                                NumberPerson = e.NumberPerson,
                                Status = e.Status,
                                Image = e.Image
                            };
            if(name != null)
            {
                listEvent = listEvent.Where(e => e.EventName.Contains(name));
            }
            return listEvent.ToList();
        }
        public List<EventDTO> getAllEvents() 
        {
            var listEvent = from ue in _context.UserEvents
                            join e in _context.Events on ue.EventId equals e.EventId
                            join u in _context.Users on ue.UserId equals u.UserId
                            join c in _context.EventCategories on e.CategoryId equals c.CategoryId
                            select new EventDTO
                            {
                                EventId = e.EventId,
                                EventName = e.EventName,
                                CategoryName = c.CategoryName,
                                CategoryId = c.CategoryId,
                                EventDate = e.EventDate,
                                EventDescription = e.EventDescription,
                                Location = e.Location,
                                NumberPerson = e.NumberPerson,
                                Status = e.Status,
                                Image = e.Image
                            };
            return listEvent.ToList();
        }
        public void UpdateRealTime()
        {
            var events = getAllEvents();
            var eventmapp = _mapper.Map<List<Event>>(events);
            foreach (Event e in eventmapp)
            {
                if (e.EventDate < DateTime.UtcNow)
                {
                    e.Status = "Completed";
                    _context.Update(e);
                    _context.SaveChanges();
                }
            }
        }

       
    }
}
