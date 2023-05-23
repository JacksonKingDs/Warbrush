using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CircularOffset
{
    public static IntXY[] Ring8 = new IntXY[]
   {
new IntXY(-2, 7),   new IntXY(-1, 7),    new IntXY(0, 7), new IntXY(1, 7),   new IntXY(2, 7),    
new IntXY(-4, 6),   new IntXY(-3, 6),   new IntXY(-2, 6),   new IntXY(4, 6),   new IntXY(3, 6),   new IntXY(2, 6),
new IntXY(-5, 5),   new IntXY(-4, 5),   new IntXY(5, 5),   new IntXY(4, 5),
new IntXY(-6, 4),   new IntXY(6, 4),
new IntXY(-6, 3),   new IntXY(6, 3),
new IntXY(-7, 2),  new IntXY(-6, 2),   new IntXY(6, 2), new IntXY(7, 2),
new IntXY(-7, 1),   new IntXY(7, 1),

new IntXY(-7, 0),                                               new IntXY(7, 0),

new IntXY(-7, -1),                                              new IntXY(7, -1),
new IntXY(-7, -2),  new IntXY(-6, -2),                          new IntXY(6, -2), new IntXY(7, -2),
new IntXY(-6, -3),                                  new IntXY(6, -3),
new IntXY(-6, -4),                               new IntXY(6, -4),
new IntXY(-5, -5),   new IntXY(-4, -5),                         new IntXY(5, -5),   new IntXY(4, -5),
new IntXY(-4, -6),   new IntXY(-3, -6),   new IntXY(-2, -6),                            new IntXY(4, -6),   new IntXY(3, -6),   new IntXY(2, -6),
new IntXY(-2, -7),   new IntXY(-1, -7),    new IntXY(0, -7), new IntXY(1, -7),   new IntXY(2, -7)
   };

    public static IntXY[] Ring7 = new IntXY[]
{
new IntXY(-1, 6),   new IntXY(0, 6),   new IntXY(1, 6),  
new IntXY(-3, 5),   new IntXY(-2, 5),   new IntXY(2, 5),   new IntXY(3, 5),
new IntXY(-4, 4),   new IntXY(4, 4),
new IntXY(-5, 3),   new IntXY(5, 3),
new IntXY(-5, 2),   new IntXY(5, 2),  
new IntXY(-6, 1),   new IntXY(6, 1),
new IntXY(-6, 0),   new IntXY(6, 0),

new IntXY(-6, -1),   new IntXY(6, -1),
new IntXY(-5, -2),   new IntXY(5, -2),
new IntXY(-5, -3),   new IntXY(5, -3),
new IntXY(-4, -4),   new IntXY(4, -4),
new IntXY(-3, -5),   new IntXY(-2, -5),   new IntXY(2, -5),   new IntXY(3, -5),
new IntXY(-1, -6),   new IntXY(0, -6),   new IntXY(1, -6)
};


    public static IntXY[] Ring6 = new IntXY[]
{
                                                                    new IntXY(0, 3),
                            new IntXY(-2, 2),   new IntXY(-1, 2),       new IntXY(1, 2),    new IntXY(2, 2),
                            new IntXY(-2, 1),      new IntXY(2, 1),
       new IntXY(-3, 0),        new IntXY(3, 0),
                            new IntXY(-2, -1),     new IntXY(2, -1),
                            new IntXY(-2, -2),  new IntXY(-1, -2),    new IntXY(1, -2),   new IntXY(2, -2),
                                                                    new IntXY(0, -3)
};


    public static IntXY[] InnerRing8 = new IntXY[]
{
new IntXY(-1, 6),   new IntXY(0, 6),   new IntXY(1, 6),
new IntXY(-3, 5),   new IntXY(-2, 5),  new IntXY(-1, 5),   new IntXY(0, 5),  new IntXY(1, 5),   new IntXY(2, 5), new IntXY(3, 5),
new IntXY(-5, 4),   new IntXY(-4, 4),new IntXY(-3, 4),   new IntXY(-2, 4),new IntXY(-1, 4),new IntXY(0, 4),new IntXY(1, 4),   new IntXY(2, 4),new IntXY(3, 4),   new IntXY(4, 4),new IntXY(5, 4),
new IntXY(-5, 3),   new IntXY(-4, 3),new IntXY(-3, 3),   new IntXY(-2, 3),new IntXY(-1, 3),new IntXY(0, 3),new IntXY(1, 3),   new IntXY(2, 3),new IntXY(3, 3),   new IntXY(4, 3),new IntXY(5, 3),
new IntXY(-5, 2),   new IntXY(-4, 2),new IntXY(-3, 2),   new IntXY(-2, 2),new IntXY(-1, 2),new IntXY(0, 2),new IntXY(1, 2),   new IntXY(2, 2),new IntXY(3, 2),   new IntXY(4, 2),new IntXY(5, 2),
new IntXY(-6, 1), new IntXY(-5, 1),   new IntXY(-4, 1),new IntXY(-3, 1),   new IntXY(-2, 1),new IntXY(-1, 1),new IntXY(0, 1),new IntXY(1, 1),   new IntXY(2,1),new IntXY(3, 1),   new IntXY(4, 1),new IntXY(5, 1), new IntXY(6, 1),
new IntXY(-6, 0), new IntXY(-5, 0),   new IntXY(-4,0),new IntXY(-3, 0),   new IntXY(-2, 0),new IntXY(-1, 0),new IntXY(0, 0),new IntXY(1, 0),   new IntXY(2,0),new IntXY(3,0),   new IntXY(4, 0),new IntXY(5, 0), new IntXY(6,0),

new IntXY(-6, -1),   new IntXY(-5, -1),   new IntXY(-4, -1),new IntXY(-3, -1),   new IntXY(-2, -1),new IntXY(-1, -1),new IntXY(0, -1),new IntXY(1, -1),   new IntXY(2, -1),new IntXY(3, -1),   new IntXY(4, -1),new IntXY(5, -1), new IntXY(6, -1),
new IntXY(-5, -2),   new IntXY(-4, -2),new IntXY(-3, -2),   new IntXY(-2, -2),new IntXY(-1, -2),new IntXY(0, -2),new IntXY(1, -2),   new IntXY(2, -2),new IntXY(3, -2),   new IntXY(4, -2),new IntXY(5, -2),
new IntXY(-5, -3),   new IntXY(-4, -3),new IntXY(-3, -3),   new IntXY(-2, -3),new IntXY(-1, -3),new IntXY(0, -3),new IntXY(1, -3),   new IntXY(2, -3),new IntXY(3, -3),   new IntXY(4, -3),new IntXY(5, -3),
new IntXY(-5, -4),   new IntXY(-4, -4),new IntXY(-3, -4),   new IntXY(-2, -4),new IntXY(-1, -4),new IntXY(0, -4),new IntXY(1, -4),   new IntXY(2, -4),new IntXY(3, -4),   new IntXY(4, -4),new IntXY(5, -4),
new IntXY(-3, -5),   new IntXY(-2, -5),  new IntXY(-1, -5),   new IntXY(0, -5),  new IntXY(1, -5),   new IntXY(2, -5), new IntXY(3, -5),
new IntXY(-1, -6),   new IntXY(0, -6),   new IntXY(1, -6)

};

    public static IntXY[] InnerRing6 = new IntXY[]
{
new IntXY(0, 2),   
                      new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),
 new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),
                            new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),  
                           new IntXY(0, -2), 
};

    public static IntXY[] XTorch = new IntXY[]
{   new IntXY(-8, 8),   new IntXY(-7, 8),   new IntXY(-6, 8),                                                                                                                                                                                                               new IntXY(6, 8),    new IntXY(7, 8),   new IntXY(8, 8),
    new IntXY(-8, 7),   new IntXY(-7, 7),   new IntXY(-6, 7),   new IntXY(-5, 7),                                                                                                                                                                       new IntXY(5, 7),    new IntXY(6, 7),    new IntXY(7, 7),    new IntXY(8, 7),
    new IntXY(-8, 6),   new IntXY(-7, 6),   new IntXY(-6, 6),   new IntXY(-5, 6),   new IntXY(-4, 6),                                                                                                                               new IntXY(4, 6),    new IntXY(5, 6),    new IntXY(6, 6),    new IntXY(7, 6),    new IntXY(8, 6),
                        new IntXY(-7, 5),   new IntXY(-6, 5),   new IntXY(-5, 5),   new IntXY(-4, 5), new IntXY(-3, 5),                                                                                         new IntXY(3, 5),    new IntXY(4, 5),    new IntXY(5, 5),    new IntXY(6, 5),    new IntXY(7, 5),    
                                            new IntXY(-6, 4),   new IntXY(-5, 4),   new IntXY(-4, 4), new IntXY(-3, 4), new IntXY(-2, 4), new IntXY(-1, 4), new IntXY(0, 4), new IntXY(1, 4), new IntXY(2, 4),  new IntXY(3, 4),    new IntXY(4, 4),    new IntXY(5, 4),    new IntXY(6, 4),
                                                                new IntXY(-5, 3),   new IntXY(-4, 3), new IntXY(-3, 3), new IntXY(-2, 3), new IntXY(-1, 3), new IntXY(0, 3), new IntXY(1, 3), new IntXY(2, 3),  new IntXY(3, 3),    new IntXY(4, 3),    new IntXY(5, 3),
                                                                                    new IntXY(-4, 2), new IntXY(-3, 2), new IntXY(-2, 2), new IntXY(-1, 2), new IntXY(0, 2), new IntXY(1, 2), new IntXY(2, 2),  new IntXY(3, 2),    new IntXY(4, 2),
                                                                                    new IntXY(-4, 1), new IntXY(-3, 1), new IntXY(-2, 1), new IntXY(-1, 1), new IntXY(0, 1), new IntXY(1, 1), new IntXY(2, 1),  new IntXY(3, 1),    new IntXY(4, 1),
                                                                                    new IntXY(-4, 0), new IntXY(-3, 0), new IntXY(-2, 0), new IntXY(-1, 0), new IntXY(0, 0), new IntXY(1, 0), new IntXY(2, 0),  new IntXY(3, 0),    new IntXY(4, 0),

    new IntXY(-8, -8),   new IntXY(-7, -8),   new IntXY(-6, -8),                                                                                                                                                                                                                          new IntXY(6, -8),    new IntXY(7, -8),   new IntXY(8, -8),
    new IntXY(-8, -7),   new IntXY(-7, -7),   new IntXY(-6, -7),   new IntXY(-5, -7),                                                                                                                                                                                new IntXY(5, -7),    new IntXY(6, -7),    new IntXY(7, -7),   new IntXY(8, -7),
    new IntXY(-8, -6),   new IntXY(-7, -6),   new IntXY(-6, -6),   new IntXY(-5, -6),   new IntXY(-4, -6),                                                                                                                                      new IntXY(4, -6),    new IntXY(5, -6),    new IntXY(6, -6),    new IntXY(7, -6),   new IntXY(8, -6),
                         new IntXY(-7, -5),   new IntXY(-6, -5),   new IntXY(-5, -5),   new IntXY(-4, -5), new IntXY(-3, -5),                                                                                               new IntXY(3, -5),   new IntXY(4, -5),    new IntXY(5, -5),    new IntXY(6, -5),    new IntXY(7, -5),
                                              new IntXY(-6, -4),   new IntXY(-5, -4),   new IntXY(-4, -4), new IntXY(-3, -4), new IntXY(-2, -4), new IntXY(-1, -4), new IntXY(0, -4), new IntXY(1, -4), new IntXY(2, -4),   new IntXY(3, -4),   new IntXY(4, -4),    new IntXY(5, -4),    new IntXY(6, -4),
                                                                   new IntXY(-5, -3),   new IntXY(-4, -3), new IntXY(-3, -3), new IntXY(-2, -3), new IntXY(-1, -3), new IntXY(0, -3), new IntXY(1, -3), new IntXY(2, -3),   new IntXY(3, -3),   new IntXY(4, -3),    new IntXY(5, -3),
                                                                                        new IntXY(-4, -2), new IntXY(-3, -2), new IntXY(-2, -2), new IntXY(-1, -2), new IntXY(0, -2), new IntXY(1, -2), new IntXY(2, -2),   new IntXY(3, -2),   new IntXY(4, -2),
                                                                                        new IntXY(-4, -1), new IntXY(-3, -1), new IntXY(-2, -1), new IntXY(-1, -1), new IntXY(0, -1), new IntXY(1, -1), new IntXY(2, -1),   new IntXY(3, -1),   new IntXY(4, -1),
};

    public static IntXY[] Circle17 = new IntXY[]
{                                                                                                   new IntXY(-3, 8), new IntXY(-2, 8), new IntXY(-1, 8), new IntXY(0, 8),
                                                                new IntXY(-5, 7), new IntXY(-4, 7), new IntXY(-3, 7), new IntXY(-2, 7), new IntXY(-1, 7), new IntXY(0, 7),
                                            new IntXY(-6, 6),   new IntXY(-5, 6), new IntXY(-4, 6), new IntXY(-3, 6), new IntXY(-2, 6), new IntXY(-1, 6), new IntXY(0, 6),
                        new IntXY(-7, 5),   new IntXY(-6, 5),   new IntXY(-5, 5), new IntXY(-4, 5), new IntXY(-3, 5), new IntXY(-2, 5), new IntXY(-1, 5), new IntXY(0, 5),
                        new IntXY(-7, 4),   new IntXY(-6, 4),   new IntXY(-5, 4), new IntXY(-4, 4), new IntXY(-3, 4), new IntXY(-2, 4), new IntXY(-1, 4), new IntXY(0, 4),
    new IntXY(-8, 3),   new IntXY(-7, 3),   new IntXY(-6, 3),   new IntXY(-5, 3), new IntXY(-4, 3), new IntXY(-3, 3), new IntXY(-2, 3), new IntXY(-1, 3), new IntXY(0, 3),
    new IntXY(-8, 2),   new IntXY(-7, 2),   new IntXY(-6, 2),   new IntXY(-5, 2), new IntXY(-4, 2), new IntXY(-3, 2), new IntXY(-2, 2), new IntXY(-1, 2), new IntXY(0, 2),
    new IntXY(-8, 1),   new IntXY(-7, 1),   new IntXY(-6, 1),   new IntXY(-5, 1), new IntXY(-4, 1), new IntXY(-3, 1), new IntXY(-2, 1), new IntXY(-1, 1), new IntXY(0, 1),
    new IntXY(-8, 0),   new IntXY(-7, 0),   new IntXY(-6, 0),   new IntXY(-5, 0), new IntXY(-4, 0), new IntXY(-3, 0), new IntXY(-2, 0), new IntXY(-1, 0), new IntXY(0, 0),

                                                                                                   new IntXY(3, 8), new IntXY(2, 8), new IntXY(1, 8),
                                                                 new IntXY(5, 7), new IntXY(4, 7), new IntXY(3, 7), new IntXY(2, 7), new IntXY(1, 7),
                                              new IntXY(6, 6),   new IntXY(5, 6), new IntXY(4, 6), new IntXY(3, 6), new IntXY(2, 6), new IntXY(1, 6),
                           new IntXY(7, 5),   new IntXY(6, 5),   new IntXY(5, 5), new IntXY(4, 5), new IntXY(3, 5), new IntXY(2, 5), new IntXY(1, 5),
                           new IntXY(7, 4),   new IntXY(6, 4),   new IntXY(5, 4), new IntXY(4, 4), new IntXY(3, 4), new IntXY(2, 4), new IntXY(1, 4),
        new IntXY(8, 3),   new IntXY(7, 3),   new IntXY(6, 3),   new IntXY(5, 3), new IntXY(4, 3), new IntXY(3, 3), new IntXY(2, 3), new IntXY(1, 3),
        new IntXY(8, 2),   new IntXY(7, 2),   new IntXY(6, 2),   new IntXY(5, 2), new IntXY(4, 2), new IntXY(3, 2), new IntXY(2, 2), new IntXY(1, 2),
        new IntXY(8, 1),   new IntXY(7, 1),   new IntXY(6, 1),   new IntXY(5, 1), new IntXY(4, 1), new IntXY(3, 1), new IntXY(2, 1), new IntXY(1, 1),
        new IntXY(8, 0),   new IntXY(7, 0),   new IntXY(6, 0),   new IntXY(5, 0), new IntXY(4, 0), new IntXY(3, 0), new IntXY(2, 0), new IntXY(1, 0), 

                                                                                                         new IntXY(-3, -8), new IntXY(-2, -8), new IntXY(-1, -8), new IntXY(0, -8),
                                                                   new IntXY(-5, -7), new IntXY(-4, -7), new IntXY(-3, -7), new IntXY(-2, -7), new IntXY(-1, -7), new IntXY(0, -7),
                                              new IntXY(-6, -6),   new IntXY(-5, -6), new IntXY(-4, -6), new IntXY(-3, -6), new IntXY(-2, -6), new IntXY(-1, -6), new IntXY(0, -6),
                         new IntXY(-7, -5),   new IntXY(-6, -5),   new IntXY(-5, -5), new IntXY(-4, -5), new IntXY(-3, -5), new IntXY(-2, -5), new IntXY(-1, -5), new IntXY(0, -5),
                         new IntXY(-7, -4),   new IntXY(-6, -4),   new IntXY(-5, -4), new IntXY(-4, -4), new IntXY(-3, -4), new IntXY(-2, -4), new IntXY(-1, -4), new IntXY(0, -4),
    new IntXY(-8, -3),   new IntXY(-7, -3),   new IntXY(-6, -3),   new IntXY(-5, -3), new IntXY(-4, -3), new IntXY(-3, -3), new IntXY(-2, -3), new IntXY(-1, -3), new IntXY(0, -3),
    new IntXY(-8, -2),   new IntXY(-7, -2),   new IntXY(-6, -2),   new IntXY(-5, -2), new IntXY(-4, -2), new IntXY(-3, -2), new IntXY(-2, -2), new IntXY(-1, -2), new IntXY(0, -2),
    new IntXY(-8, -1),   new IntXY(-7, -1),   new IntXY(-6, -1),   new IntXY(-5, -1), new IntXY(-4, -1), new IntXY(-3, -1), new IntXY(-2, -1), new IntXY(-1, -1), new IntXY(0, -1),
    new IntXY(-8, -0),   new IntXY(-7, -0),   new IntXY(-6, -0),   new IntXY(-5, -0), new IntXY(-4, -0), new IntXY(-3, -0), new IntXY(-2, -0), new IntXY(-1, -0), new IntXY(0, -0),

                                                                                                        new IntXY(3, -8), new IntXY(2, -8), new IntXY(1, -8),
                                                                    new IntXY(5, -7), new IntXY(4, -7), new IntXY(3, -7), new IntXY(2, -7), new IntXY(1, -7),
                                                new IntXY(6, -6),   new IntXY(5, -6), new IntXY(4, -6), new IntXY(3, -6), new IntXY(2, -6), new IntXY(1, -6),
                            new IntXY(7, -5),   new IntXY(6, -5),   new IntXY(5, -5), new IntXY(4, -5), new IntXY(3, -5), new IntXY(2, -5), new IntXY(1, -5),
                            new IntXY(7, -4),   new IntXY(6, -4),   new IntXY(5, -4), new IntXY(4, -4), new IntXY(3, -4), new IntXY(2, -4), new IntXY(1, -4),
        new IntXY(8, -3),   new IntXY(7, -3),   new IntXY(6, -3),   new IntXY(5, -3), new IntXY(4, -3), new IntXY(3, -3), new IntXY(2, -3), new IntXY(1, -3),
        new IntXY(8, -2),   new IntXY(7, -2),   new IntXY(6, -2),   new IntXY(5, -2), new IntXY(4, -2), new IntXY(3, -2), new IntXY(2, -2), new IntXY(1, -2),
        new IntXY(8, -1),   new IntXY(7, -1),   new IntXY(6, -1),   new IntXY(5, -1), new IntXY(4, -1), new IntXY(3, -1), new IntXY(2, -1), new IntXY(1, -1),
};

    public static IntXY[] Circle10 = new IntXY[]
    {                                                                                   new IntXY(-2, 6),   new IntXY(-1, 6),   new IntXY(0, 6),    new IntXY(1, 6),    new IntXY(2, 6),
                                                new IntXY(-4, 5),   new IntXY(-3, 5),   new IntXY(-2, 5),   new IntXY(-1, 5),   new IntXY(0, 5),    new IntXY(1, 5),    new IntXY(2, 5),    new IntXY(3, 5),    new IntXY(4, 5),
                            new IntXY(-5, 4),   new IntXY(-4, 4),   new IntXY(-3, 4),   new IntXY(-2, 4),   new IntXY(-1, 4),   new IntXY(0, 4),    new IntXY(1, 4),    new IntXY(2, 4),    new IntXY(3, 4),    new IntXY(4, 4),   new IntXY(5, 4),
                            new IntXY(-5, 3),   new IntXY(-4, 3),   new IntXY(-3, 3),   new IntXY(-2, 3),   new IntXY(-1, 3),   new IntXY(0, 3),    new IntXY(1, 3),    new IntXY(2, 3),    new IntXY(3, 3),    new IntXY(4, 3),   new IntXY(5, 3),
        new IntXY(-6, 2),   new IntXY(-5, 2),   new IntXY(-4, 2),   new IntXY(-3, 2),   new IntXY(-2, 2),   new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    new IntXY(2, 2),    new IntXY(3, 2),    new IntXY(4, 2),   new IntXY(5, 2),   new IntXY(6, 2),
        new IntXY(-6, 1),   new IntXY(-5, 1),   new IntXY(-4, 1),   new IntXY(-3, 1),   new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),    new IntXY(3, 1),    new IntXY(4, 1),   new IntXY(5, 1),   new IntXY(6, 1),
        new IntXY(-6, 0),   new IntXY(-5, 0),   new IntXY(-4, 0),   new IntXY(-3, 0),   new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0),    new IntXY(4, 0),   new IntXY(5, 0),   new IntXY(6, 0),

        new IntXY(-6, -1),   new IntXY(-5, -1),   new IntXY(-4, -1),   new IntXY(-3, -1),   new IntXY(-2, -1),   new IntXY(-1, -1),   new IntXY(0, -1),    new IntXY(1, -1),    new IntXY(2, -1),    new IntXY(3, -1),    new IntXY(4, -1),   new IntXY(5, -1),   new IntXY(6, -1),
        new IntXY(-6, -2),   new IntXY(-5, -2),   new IntXY(-4, -2),   new IntXY(-3, -2),   new IntXY(-2, -2),   new IntXY(-1, -2),   new IntXY(0, -2),    new IntXY(1, -2),    new IntXY(2, -2),    new IntXY(3, -2),    new IntXY(4, -2),   new IntXY(5, -2),   new IntXY(6, -2),
                             new IntXY(-5, -3),   new IntXY(-4, -3),   new IntXY(-3, -3),   new IntXY(-2, -3),   new IntXY(-1, -3),   new IntXY(0, -3),    new IntXY(1, -3),    new IntXY(2, -3),    new IntXY(3, -3),    new IntXY(4, -3),   new IntXY(5, -3),
                             new IntXY(-5, -4),   new IntXY(-4, -4),   new IntXY(-3, -4),   new IntXY(-2, -4),   new IntXY(-1, -4),   new IntXY(0, -4),    new IntXY(1, -4),    new IntXY(2, -4),    new IntXY(3, -4),    new IntXY(4, -4),   new IntXY(5, -4),
                                                  new IntXY(-4, -5),   new IntXY(-3, -5),   new IntXY(-2, -5),   new IntXY(-1, -5),   new IntXY(0, -5),    new IntXY(1, -5),    new IntXY(2, -5),    new IntXY(3, -5),    new IntXY(4, -5), 
                                                                                            new IntXY(-2, -6),   new IntXY(-1, -6),   new IntXY(0, -6),    new IntXY(1, -6),    new IntXY(2, -6)
  };

    public static IntXY[] Circle9 = new IntXY[] 
    {   
                                                                                        new IntXY(-1, 5),   new IntXY(0, 5),    new IntXY(1, 5), 
                                                new IntXY(-3, 4),   new IntXY(-2, 4),   new IntXY(-1, 4),   new IntXY(0, 4),    new IntXY(1, 4),    new IntXY(2, 4),    new IntXY(3, 4), 
                            new IntXY(-4, 3),   new IntXY(-3, 3),   new IntXY(-2, 3),   new IntXY(-1, 3),   new IntXY(0, 3),    new IntXY(1, 3),    new IntXY(2, 3),    new IntXY(3, 3),    new IntXY(4, 3), 
                            new IntXY(-4, 2),   new IntXY(-3, 2),   new IntXY(-2, 2),   new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    new IntXY(2, 2),    new IntXY(3, 2),    new IntXY(4, 2),  
        new IntXY(-5, 1),   new IntXY(-4, 1),   new IntXY(-3, 1),   new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),    new IntXY(3, 1),    new IntXY(4, 1),    new IntXY(5, 1),
        new IntXY(-5, 0),   new IntXY(-4, 0),   new IntXY(-3, 0),   new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0),    new IntXY(4, 0),    new IntXY(5, 0),
        new IntXY(-5, -1),  new IntXY(-4, -1),  new IntXY(-3, -1),  new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),   new IntXY(3, -1),   new IntXY(4, -1),   new IntXY(5, -1),
                            new IntXY(-4, -2),  new IntXY(-3, -2),  new IntXY(-2, -2),  new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2),   new IntXY(2, -2),   new IntXY(3, -2),   new IntXY(4, -2),  
                            new IntXY(-4, -3),  new IntXY(-3, -3),  new IntXY(-2, -3),  new IntXY(-1, -3),  new IntXY(0, -3),   new IntXY(1, -3),   new IntXY(2, -3),   new IntXY(3, -3),   new IntXY(4, -3),   
                                                new IntXY(-3, -4),  new IntXY(-2, -4),  new IntXY(-1, -4),  new IntXY(0, -4),   new IntXY(1, -4),   new IntXY(2, -4),   new IntXY(3, -4),   
                                                                                        new IntXY(-1, -5),  new IntXY(0, -5),   new IntXY(1, -5)   
    };

    public static IntXY[] Circle8 = new IntXY[]
    {
                                                                    new IntXY(-1, 4),   new IntXY(0, 4),    new IntXY(1, 4),    
                                                new IntXY(-2, 3),   new IntXY(-1, 3),   new IntXY(0, 3),    new IntXY(1, 3),    new IntXY(2, 3),   
                            new IntXY(-3, 2),   new IntXY(-2, 2),   new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    new IntXY(2, 2),    new IntXY(3, 2),    
        new IntXY(-4, 1),   new IntXY(-3, 1),   new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),    new IntXY(3, 1),    new IntXY(4, 1), 
        new IntXY(-4, 0),   new IntXY(-3, 0),   new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0),    new IntXY(4, 0), 
        new IntXY(-4, -1),  new IntXY(-3, -1),  new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),   new IntXY(3, -1),   new IntXY(4, -1),
                            new IntXY(-3, -2),  new IntXY(-2, -2),  new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2),   new IntXY(2, -2),   new IntXY(3, -2),   
                                                new IntXY(-2, -3),  new IntXY(-1, -3),  new IntXY(0, -3),   new IntXY(1, -3),   new IntXY(2, -3),   
                                                                    new IntXY(-1, -4),  new IntXY(0, -4),   new IntXY(1, -4) 
    };

    public static IntXY[] Circle7_to_8Ring = new IntXY[]
{
                                                                    new IntXY(-1, 4),   new IntXY(0, 4),    new IntXY(1, 4),
                                                new IntXY(-2, 3),                                                               new IntXY(2, 3),
                            new IntXY(-3, 2),                                                                                                       new IntXY(3, 2),
        new IntXY(-4, 1),                                                                                                                                              new IntXY(4, 1),
        new IntXY(-4, 0),                                                                                                                                              new IntXY(4, 0),
        new IntXY(-4, -1),                                                                                                                                             new IntXY(4, -1),
                            new IntXY(-3, -2),                                                                                                      new IntXY(3, -2),
                                                new IntXY(-2, -3),                                                              new IntXY(2, -3),
                                                                    new IntXY(-1, -4),  new IntXY(0, -4),   new IntXY(1, -4)
};

    public static IntXY[] Circle7 = new IntXY[]
    {
                                                new IntXY(-1, 3),   new IntXY(0, 3),    new IntXY(1, 3),    
                            new IntXY(-2, 2),   new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    new IntXY(2, 2),    
       new IntXY(-3, 1),    new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),    new IntXY(3, 1), 
       new IntXY(-3, 0),    new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0), 
       new IntXY(-3, -1),   new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),   new IntXY(3, -1),
                            new IntXY(-2, -2),  new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2),   new IntXY(2, -2),  
                                                new IntXY(-1, -3),  new IntXY(0, -3),   new IntXY(1, -3)
    };

    public static IntXY[] Circle6 = new IntXY[]
    {
                                                                    new IntXY(0, 3),    
                            new IntXY(-2, 2),   new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    new IntXY(2, 2),
                            new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),    
       new IntXY(-3, 0),    new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0),
                            new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),   
                            new IntXY(-2, -2),  new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2),   new IntXY(2, -2),
                                                                    new IntXY(0, -3)   
    };

    public static IntXY[] Circle5B = new IntXY[]
    {
                                                                    new IntXY(0, 3),
                                                new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),    
                            new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1),
       new IntXY(-3, 0),    new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),    new IntXY(3, 0),
                            new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),
                                                new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2),   
                                                                    new IntXY(0, -3)
    };

    public static IntXY[] Circle5 = new IntXY[]
    {
                                                new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2), 
                            new IntXY(-2, 1),   new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    new IntXY(2, 1), 
                            new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0), 
                            new IntXY(-2, -1),  new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   new IntXY(2, -1),
                                                new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2)
    };

    public static IntXY[] Circle4 = new IntXY[]
{
                            new IntXY(-1, 2),   new IntXY(0, 2),    new IntXY(1, 2),
                            new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),    
        new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),
                            new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   
                            new IntXY(-1, -2),  new IntXY(0, -2),   new IntXY(1, -2)
};

    public static IntXY[] Circle3 = new IntXY[]
    {
                                                new IntXY(0, 2),        
                            new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),
        new IntXY(-2, 0),   new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),    new IntXY(2, 0),
                            new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),   
                                                new IntXY(0, -2), 
    };

    public static IntXY[] Circle2 = new IntXY[]
{
                            new IntXY(-1, 1),   new IntXY(0, 1),    new IntXY(1, 1),
                            new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0),  
                            new IntXY(-1, -1),  new IntXY(0, -1),   new IntXY(1, -1),
};

    public static IntXY[] Circle1 = new IntXY[]
    {
                                                new IntXY(0, 1),    
                            new IntXY(-1, 0),   new IntXY(0, 0),    new IntXY(1, 0), 
                                                new IntXY(0, -1)    
    };


    public static IntXY[] Circle0 = new IntXY[]
    {
                            new IntXY(0, 0)
    };


    public static IntXY[][] Circles = 
    {
        Circle0, Circle0, Circle0, Circle1, Circle1, Circle2, Circle3, Circle3, Circle4, Circle5, Circle5B, Circle6, Circle7, Circle8, Circle9
    };


    public static IntXY[] Heart = new IntXY[]
{
    new IntXY(-3, 3), new IntXY(-2, 3),
    new IntXY(-4, 2), new IntXY(-1, 2),
    new IntXY(-4, 1), new IntXY(0, 1),
    new IntXY(-3, 0),
    new IntXY(-3, -1),
    new IntXY(-2, -2),
    new IntXY(-1, -3),
    new IntXY(0, -4),

    new IntXY(3, 3), new IntXY(2, 3),
    new IntXY(4, 2), new IntXY(1, 2),
    new IntXY(4, 1), 
    new IntXY(3, 0),
    new IntXY(3, -1),
    new IntXY(2, -2),
    new IntXY(1, -3)
};

    public static IntXY[] Trophy = new IntXY[]
{
    new IntXY(-2, 4), new IntXY(-1, 4), new IntXY(0, 4),
    new IntXY(-4, 3), new IntXY(-3, 3),new IntXY(-2, 3),
    new IntXY(-4, 2),new IntXY(-2, 2),
    new IntXY(-4, 1), new IntXY(-2, 1),
    new IntXY(-3, 0), new IntXY(-2, 0),
    new IntXY(-1, -1),
    new IntXY(0,  -2),
    new IntXY(-1, -3),
    new IntXY(-2, -4),
    new IntXY(-2, -5), new IntXY(-1, -5), new IntXY(0, -5),

    new IntXY(2, 4), new IntXY(1, 4), 
    new IntXY(4, 3), new IntXY(3, 3),new IntXY(2, 3),
    new IntXY(4, 2),new IntXY(2, 2),
    new IntXY(4, 1), new IntXY(2, 1),
    new IntXY(3, 0), new IntXY(2, 0),
    new IntXY(1, -1),
    new IntXY(1, -3),
    new IntXY(2, -4),
    new IntXY(2, -5), new IntXY(1, -5), 
};

    public static IntXY[] Cupcake = new IntXY[]
{
    new IntXY(-1, 4), new IntXY(0, 4),
    new IntXY(-2, 3), 
    new IntXY(-3, 2),new IntXY(-1, 2), new IntXY(0, 2),
    new IntXY(-4, 1),  
    new IntXY(-4, 0), 
    new IntXY(-4, -1),
    new IntXY(-3, -2), new IntXY(-2, -2), new IntXY(-1, -2),new IntXY(0, -2),
    new IntXY(-3, -3),
    new IntXY(-2, -4), new IntXY(-1, -4), new IntXY(0, -4),

    new IntXY(1, 4), 
    new IntXY(2, 3),
    new IntXY(3, 2),new IntXY(1, 2), 
    new IntXY(4, 1),
    new IntXY(4, 0),
    new IntXY(4, -1),
    new IntXY(3, -2), new IntXY(2, -2), new IntXY(1, -2),
    new IntXY(3, -3),
    new IntXY(2, -4), new IntXY(1, -4), 
};

    public static IntXY[] Clover = new IntXY[]
{
    new IntXY(-2, 4), new IntXY(-1, 4),
    new IntXY(-3, 3), new IntXY(0, 3),
    new IntXY(-4, 2),
    new IntXY(-4, 1), new IntXY(0, 1),
    new IntXY(-3, 0), new IntXY(-1, 0),

    new IntXY(2, 4), new IntXY(1, 4),
    new IntXY(3, 3), new IntXY(1, 3),
    new IntXY(4, 2),
    new IntXY(4, 1), 
    new IntXY(3, 0), new IntXY(1, 0),

    new IntXY(-2, -4), new IntXY(-1, -4),
    new IntXY(-3, -3), new IntXY(0, -3),
    new IntXY(-4, -2),
    new IntXY(-4, -1), new IntXY(0, -1),

    new IntXY(2, -4), new IntXY(1, -4),
    new IntXY(3, -3), new IntXY(1, -3),
    new IntXY(4, -2),
    new IntXY(4, -1), 
};
}
