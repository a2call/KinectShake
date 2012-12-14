using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    class Atom
    {
        public enum AtomType { H, O };

        float [] pos_ = new float[3];
        AtomType t_ = AtomType.H;
        public static Color [] atomcolors_ = { Color.White, Color.Red };
        public static float [] atomradii_ = {0.3f,0.7f};


        public Atom(AtomType t)
        {
            t_ = t;
        }

        public float x(){return pos_[0];}
        public float y(){return pos_[1];}
        public float z(){return pos_[2];}

        public float r(){return atomradii_[(int)t_];}
        public Color color() { return atomcolors_[(int)t_]; }


        public void setPos(float x, float y, float z)
        {
            pos_[0] = x;
            pos_[1] = y;
            pos_[2] = z;
        }
               
        public bool intersect(Atom a){
            float dx = pos_[0] - a.pos_[0];
            float dy = pos_[1] - a.pos_[1];
            float dz = pos_[2] - a.pos_[2];

            float d = (float) Math.Sqrt(dx * dx + dy * dy + dz * dz);

            if (d > (a.r() + r()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    class AtomsDB
    {
        public ArrayList atomlist = new ArrayList();

        public void addAtom(Atom atm)
        {
            atomlist.Add(atm);
        }

        public bool checkIntersection(Atom a)
        {
            foreach (Atom atm in atomlist)
            {
                if (a.intersect(atm))
                {
                    return true;
                }
            }
            return false;
        }
    }
}