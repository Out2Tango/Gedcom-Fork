﻿// <copyright file="GedcomState.cs" company="GeneGenie.com">
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
// <author> Copyright (C) 2016 Ryan O'Neill r@genegenie.com </author>
// <author> Copyright (C) 2007 David A Knight david@ritter.demon.co.uk </author>

namespace GeneGenie.Gedcom.Parser.Enums
{
    /// <summary>
    /// Defines the parse states for GEDCOM file
    /// </summary>
    public enum GedcomState
    {
        /// <summary>
        /// Reading the current level
        /// </summary>
        Level,

        /// <summary>
        /// Reading the current ID
        /// </summary>
        XrefID,

        /// <summary>
        /// Reading the current tag name
        /// </summary>
        Tag,

        /// <summary>
        /// Reading the value for the current tag
        /// </summary>
        LineValue,
    }
}
