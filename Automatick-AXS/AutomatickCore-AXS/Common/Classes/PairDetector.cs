using Automatick.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class PairDetector
    {
        private List<SeatsPair> _pairs;

        public Boolean HasPairs { get; set; }
        public Boolean isGA { get; set; }

        public List<SeatsPair> Pairs
        {
            get
            {
                return this._pairs;
            }
        }

        public PairDetector(Offer msg, int degree)
        {
            _pairs = new List<SeatsPair>();

            try
            {
                //if (degree > 1)
                this.HasPairs = workoutPairs(msg, degree);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private Boolean workoutPairs(Offer msg, int degree)
        {
            Boolean result = false;
            _pairs = new List<SeatsPair>();

            try
            {
                var priceGrp = msg.items.GroupBy(t => t.priceLevelID);

                foreach (var price in priceGrp)
                {
                    var secGrp = price.GroupBy(t => t.sectionID);

                    foreach (var item in secGrp)
                    {
                        var rowGrp = item.GroupBy(r => r.rowID);

                        foreach (var rowItem in rowGrp)
                        {
                            try
                            {
                                List<int> seats = rowItem.Select(seat => Convert.ToInt32(seat.number)).ToList();
                                seats.Sort();

                                for (int i = 0; (i < seats.Count) && ((i + degree) <= seats.Count); i++)
                                {
                                    SeatsPair sp = new SeatsPair() { PriceLevelID = price.Key, SectionID = item.Key, RowID = rowItem.Key, Row = rowItem.FirstOrDefault().rowLabel, SeatType = item.FirstOrDefault().seatType, sectionLabel = rowItem.FirstOrDefault().sectionLabel, Seats = new List<int>() };
                                    List<int> possiblePair = seats.GetRange(i, degree);
                                    Boolean isDegreePair = true;

                                    for (int j = 0; (j < degree); j++)
                                    {
                                        int current = possiblePair[j];

                                        if ((j + 1) < possiblePair.Count)
                                        {
                                            if (possiblePair[j + 1] != (++current))
                                            {
                                                isDegreePair = false;
                                                break;
                                            }
                                        }
                                    }

                                    if (isDegreePair)
                                    {
                                        sp.Seats.AddRange(possiblePair);
                                        _pairs.Add(sp);
                                        result = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);

                                if (rowItem.Any(pred => pred.number.ToLower().Equals("ga") && String.IsNullOrEmpty(pred.rowLabel)))
                                {
                                    isGA = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
