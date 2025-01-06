namespace API.Extensions
{
    public static class Mapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : new()
        {
            var destination = new TDestination();

            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .FirstOrDefault(dp => dp.Name == sourceProperty.Name && dp.CanWrite);

                if (destinationProperty != null)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }

        public static List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sourceList)
            where TDestination : new()
        {
            return sourceList.Select(item => Map<TSource, TDestination>(item)).ToList();
        }
    }
}
