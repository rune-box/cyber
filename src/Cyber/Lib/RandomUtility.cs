using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.Lib {
  public class RandomUtility {
    static Random random = new Random();

    /// <summary>
    /// return value in [min, max)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int CreateRandom(int min, int max) {
      if(min > max) {
        int tmp = min;
        min = max;
        max = tmp;
      }
      return random.Next( min, max );
    }

    public static float CreateRandom(float min, float max) {
      if (min > max) {
        float tmp = min;
        min = max;
        max = tmp;
      }
      float diff = max - min;
      if (diff <= double.Epsilon)
        return min;

      double r = random.NextDouble();
      double v = min + diff * r;

      return (float)v;
    }

    public static double CreateRandom(double min, double max) {
      if (min > max) {
        double tmp = min;
        min = max;
        max = tmp;
      }
      double diff = max - min;
      if (diff <= double.Epsilon)
        return min;

      double r = random.NextDouble();
      double v = min + diff * r;

      return v;
    }

    /// <summary>
    /// return value in [0, 1.0)
    /// </summary>
    /// <returns></returns>
    public static double CreateRandom() {
      return random.NextDouble();
    }

    public static int RandomCentered(int value_og, int offset= 5) {
      int value = CreateRandom( value_og - offset, value_og + offset );
      return value;
    }

    public static float RandomCentered(float value_og, float offset = 5) {
      float value = CreateRandom( value_og - offset, value_og + offset );
      return value;
    }

    public static double RandomCentered(double value_og, double offset = 5) {
      double value = CreateRandom( value_og - offset, value_og + offset );
      return value;
    }

    /// <summary>
    /// get a random item, and remove it from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static T PopRandomElement<T>(List<T> elements) {
      if (elements == null)
        throw new ArgumentNullException( "elements" );
      else if (elements.Count == 0)
        throw new ArgumentException( "elements has no items" );

      int index = RandomUtility.CreateRandom( 0, elements.Count );
      T item = elements.ElementAt( index );

      elements.RemoveAt( index );

      return item;
    }

    /// <summary>
    /// get a random item from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static T GetRandomElement<T>(IEnumerable<T> elements) {
      if (elements == null)
        throw new ArgumentNullException( "elements" );
      int count = elements.Count();
      if (count == 0)
        throw new ArgumentException( "elements has no items" );

      int index = RandomUtility.CreateRandom( 0, count );
      T item = elements.ElementAt( index );

      return item;
    }

  }

}
