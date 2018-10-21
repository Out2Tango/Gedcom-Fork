// <copyright file="GedcomPlace.cs" company="GeneGenie.com">
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see http:www.gnu.org/licenses/ .
//
// </copyright>
// <author> Copyright (C) 2007 David A Knight david@ritter.demon.co.uk </author>
// <author> Copyright (C) 2016 Ryan O'Neill r@genegenie.com </author>

namespace GeneGenie.Gedcom
{
    using System;
    using System.IO;
    using Enums;

    /// <summary>
    /// Represents a place or location.
    /// </summary>
    /// <seealso cref="GedcomRecord" />
    public class GedcomPlace : GedcomRecord, IEquatable<GedcomPlace>, IComparable<GedcomPlace>, IComparable
    {
        private string name;
        private string form;

        private GedcomRecordList<GedcomVariation> phoneticVariations;
        private GedcomRecordList<GedcomVariation> romanizedVariations;

        private string latitude;
        private string longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomPlace"/> class.
        /// </summary>
        public GedcomPlace()
        {
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get { return GedcomRecordType.Place; }
        }

        /// <summary>
        /// Gets the GEDCOM tag for a place.
        /// </summary>
        /// <value>
        /// The GEDCOM tag.
        /// </value>
        public override string GedcomTag
        {
            get { return "PLAC"; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value != name)
                {
                    name = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public string Form
        {
            get
            {
                return form;
            }

            set
            {
                if (value != form)
                {
                    form = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the phonetic variations.
        /// </summary>
        /// <value>
        /// The phonetic variations.
        /// </value>
        public GedcomRecordList<GedcomVariation> PhoneticVariations
        {
            get
            {
                if (phoneticVariations == null)
                {
                    phoneticVariations = new GedcomRecordList<GedcomVariation>();
                    phoneticVariations.CollectionChanged += ListChanged;
                }

                return phoneticVariations;
            }
        }

        /// <summary>
        /// Gets the romanized variations.
        /// </summary>
        /// <value>
        /// The romanized variations.
        /// </value>
        public GedcomRecordList<GedcomVariation> RomanizedVariations
        {
            get
            {
                if (romanizedVariations == null)
                {
                    romanizedVariations = new GedcomRecordList<GedcomVariation>();
                    romanizedVariations.CollectionChanged += ListChanged;
                }

                return romanizedVariations;
            }
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public string Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                if (value != latitude)
                {
                    latitude = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public string Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                if (value != longitude)
                {
                    longitude = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public override GedcomChangeDate ChangeDate
        {
            get
            {
                GedcomChangeDate realChangeDate = base.ChangeDate;
                GedcomChangeDate childChangeDate;
                if (phoneticVariations != null)
                {
                    foreach (GedcomVariation variation in phoneticVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
                    }
                }

                if (romanizedVariations != null)
                {
                    foreach (GedcomVariation variation in romanizedVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
                    }
                }

                if (realChangeDate != null)
                {
                    realChangeDate.Level = Level + 2;
                }

                return realChangeDate;
            }

            set
            {
                base.ChangeDate = value;
            }
        }

        /// <summary>
        /// Outputs this instance as a GEDCOM record.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        public override void Output(TextWriter sw)
        {
            sw.Write(Environment.NewLine);
            sw.Write(Util.IntToString(Level));
            sw.Write(" PLAC ");

            if (!string.IsNullOrEmpty(Name))
            {
                string line = Name.Replace("@", "@@");
                sw.Write(line);
            }

            OutputStandard(sw);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            if (!string.IsNullOrEmpty(Form))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = Util.IntToString(Level + 1);
                }

                string line = Form.Replace("@", "@@");
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" FORM ");
                sw.Write(line);
            }

            if (phoneticVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = Util.IntToString(Level + 1);
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = Util.IntToString(Level + 2);
                }

                foreach (GedcomVariation variation in PhoneticVariations)
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusOne);
                    sw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(line);
                    }
                }
            }

            if (romanizedVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = Util.IntToString(Level + 1);
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = Util.IntToString(Level + 2);
                }

                foreach (GedcomVariation variation in RomanizedVariations)
                {
                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusOne);
                    sw.Write(" ROMN ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(variation.VariationType);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = Util.IntToString(Level + 1);
                }

                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" MAP");
                if (!string.IsNullOrEmpty(Latitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = Util.IntToString(Level + 2);
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusTwo);
                    sw.Write(" LATI ");
                    string line = Latitude.Replace("@", "@@");
                    sw.Write(line);
                }

                if (!string.IsNullOrEmpty(Longitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = Util.IntToString(Level + 2);
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusTwo);
                    sw.Write(" LONG ");
                    string line = Longitude.Replace("@", "@@");
                    sw.Write(line);
                }
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>
        /// True if instance matches user data, otherwise false.
        /// </returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomPlace) == 0;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomPlace to compare this instance against.</param>
        /// <returns>
        /// True if instance matches user data, otherwise false.
        /// </returns>
        public bool Equals(GedcomPlace other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compares this place record to another record.
        /// </summary>
        /// <param name="place">A place record.</param>
        /// <returns>
        /// &lt;0 if the this record precedes the other in the sort order;
        /// &gt;0 if the other record precedes this one;
        /// 0 if the records are equal
        /// </returns>
        public int CompareTo(GedcomPlace place)
        {
            if (place == null)
            {
                return 1;
            }

            var compare = string.Compare(Form, place.Form);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Latitude, place.Latitude);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Longitude, place.Longitude);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Name, place.Name);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(PhoneticVariations, place.PhoneticVariations);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(RomanizedVariations, place.RomanizedVariations);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The GedcomRepositoryRecord to compare this instance against.</param>
        /// <returns>
        /// True if instance matches user data, otherwise False.
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomPlace);
        }
    }
}
