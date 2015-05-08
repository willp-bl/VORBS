﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VORBS.Models;
using VORBS.DAL;
using VORBS.Models.DTOs;

namespace VORBS.API
{
    [RoutePrefix("api/availability")]
    public class AvailabilityController : ApiController
    {
        private VORBSContext db = new VORBSContext();

        [HttpGet]
        [Route("{location}/{start:DateTime}/{end:DateTime}")]
        public List<RoomDTO> GetAvailableRoomsForLocation(string location, DateTime start, DateTime end)
        {
            List<RoomDTO> rooms = new List<RoomDTO>();

            if (location == null)
                return new List<RoomDTO>();

            List<Room> roomData = new List<Room>();
            var locationRooms = db.Rooms.Where(x => x.Location.Name == location && x.SeatCount >= 5).ToList();
            var availableRooms = db.Rooms.Where(x =>
                x.Location.Name == location
                //&& x.SeatCount >= 5
                //&& (x.Bookings.Where(b => b.StartDate < end && start < b.EndDate)).Count() == 0
            ).ToList();

            roomData.AddRange(availableRooms);

            roomData.ForEach(x => rooms.Add(new RoomDTO()
            {
                ID = x.ID,
                RoomName = x.RoomName,
                PhoneCount = x.PhoneCount,
                ComputerCount = x.ComputerCount,
                SmartRoom = x.SmartRoom,
                SeatCount = x.SeatCount,
                Bookings = x.Bookings.Where( b => b.StartDate.Date == start.Date && b.EndDate.Date == end.Date).Select(b =>
                {
                    BookingDTO bDto = new BookingDTO()
                    {
                        ID = b.ID,
                        Owner = b.Owner,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate
                    };
                    return bDto;
                }).ToList()
            }));

            return rooms;
        }

    }
}
