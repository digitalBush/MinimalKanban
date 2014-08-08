using Domain.Aggregates;

namespace Domain
{
    public static class ConflictDetectorFactory
    {
        public static ConflictDetector Create()
        {
            var detector = new ConflictDetector();
            detector.Allow<CardAdded>();
            detector.Allow<CardMoved>();
            detector.Allow<CardAdded, CardMoved>();
            detector.Allow<CardArchived>();
            return detector;
        }
    }
}