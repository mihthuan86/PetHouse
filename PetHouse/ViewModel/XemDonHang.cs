using System;
using System.Collections.Generic;
using PetHouse.Models;

namespace PetHouse.ViewModel
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public string Status
        {
            get
            {
                string stt = "";
                if (DonHang != null)
                {
                    switch (DonHang.OrderStatus)
                    {
                        case 1:
                            {
                                stt = "Đang Giao";
                                break;
                            }
                        case 2:
                            {
                                stt = "Hoàn Thành";
                                break;
                            }
                        case 0:
                            {
                                stt = "Đã Đặt";
                                break;
                            }
                        case -1:
                            {
                                stt = "Đã hủy";
                                break;
                            }
                        default:break;
                    }
                }
                return stt;
            }
        }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
