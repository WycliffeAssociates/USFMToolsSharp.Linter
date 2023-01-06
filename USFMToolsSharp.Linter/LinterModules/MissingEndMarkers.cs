using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.LinterModules
{
    class MissingEndMarkers : ILinterModule
    {
        public Dictionary<Type, Type> markerPairs;
        public List<LinterResult> Lint(USFMDocument root)
        {
            List<LinterResult> missingEndMarkers = new List<LinterResult>();
            markerPairs = new Dictionary<Type, Type>
            {
                {typeof(ADDMarker),typeof(ADDEndMarker)},
                {typeof(BDMarker),typeof(BDEndMarker)},
                {typeof(BDITMarker),typeof(BDITEndMarker)},
                {typeof(BKMarker),typeof(BKEndMarker)},
                {typeof(CAMarker),typeof(CAEndMarker)},
                {typeof(EMMarker),typeof(EMEndMarker)},
                {typeof(FVMarker),typeof(FVEndMarker)},
                {typeof(IORMarker),typeof(IOREndMarker)},
                {typeof(ITMarker),typeof(ITEndMarker)},
                {typeof(NDMarker),typeof(NDEndMarker)},
                {typeof(NOMarker),typeof(NOEndMarker)},
                {typeof(QACMarker),typeof(QACEndMarker)},
                {typeof(QSMarker),typeof(QSEndMarker)},
                {typeof(RQMarker),typeof(RQEndMarker)},
                {typeof(SCMarker),typeof(SCEndMarker)},
                {typeof(SUPMarker),typeof(SUPEndMarker)},
                {typeof(TLMarker),typeof(TLEndMarker)},
                {typeof(VAMarker),typeof(VAEndMarker)},
                {typeof(VPMarker),typeof(VPEndMarker)},
                {typeof(WMarker),typeof(WEndMarker)},
                {typeof(XMarker), typeof(XEndMarker)},
            };

            return CheckChildMarkers(root, root.Contents);

        }
        /// <summary>
        /// Iterates through all children markers
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<LinterResult> CheckChildMarkers(USFMDocument root, List<Marker> markers)
        {
            List<LinterResult> results = new List<LinterResult>();

            foreach(Marker marker in markers)
            {
                if (markerPairs.ContainsKey(marker.GetType()))
                {
                    results.AddRange(CheckEndMarker(root, marker));
                }
                results.AddRange(CheckChildMarkers(root, marker.Contents));
            }
            return results;
        }
        /// <summary>
        /// Checks Closing Marker for Unique Marker 
        /// </summary>
        /// <param name="marker"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<LinterResult> CheckEndMarker(USFMDocument root, Marker marker)
        {
            List<int> markerPositions = new List<int>();
            List<Marker> hierarchy = root.GetHierarchyToMarker(marker);
            List<Marker> siblingMarkers = hierarchy[hierarchy.Count - 2].Contents;
            foreach (Marker sibling in siblingMarkers)
            {
                if (sibling.GetType() == marker.GetType())
                {
                    markerPositions.Add(sibling.Position);
                }
                else if (sibling.GetType() == markerPairs[marker.GetType()])
                {
                    if (markerPositions.Count > 0)
                    {
                        markerPositions.RemoveAt(markerPositions.Count - 1);
                    }
                }
            }
            List<LinterResult> results = new List<LinterResult>();
            foreach(int loneMarkerPosition in markerPositions)
            {
                results.Add(new LinterResult
                {
                    Position = loneMarkerPosition,
                    Level = LinterLevel.Error,
                    Message = $"Missing closing marker for {marker.Identifier}"
                });

            }
            return results;

        }
    }

}
