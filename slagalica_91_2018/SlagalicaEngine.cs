using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slagalica_91_2018
{
    //tref 1
    //karo 2
    //pik  3
    //herc 4
    [Serializable]
    class SlagalicaEngine
    {
        int[,] matrica;
        int[] prazno;
        public int[,] Matrica { get { return matrica; } }
        public int vreme = 0;
        public int brojPoteza;
        public SlagalicaEngine()
        {
            prazno = new int[2];
            matrica = new int[4, 4];
            promesaj();            
        }

        public void promesaj()
        {
            Random rnd = new Random();

            int tref = 0;
            int karo = 0;
            int pik = 0;
            int herc = 0;

            int br;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) {

                    br = rnd.Next(1, 5);
                    
                    while ((br == 1 && tref == 4) || (br == 2 && karo == 4) || (br == 3 && pik == 4) || (br == 4 && herc == 4)) {
                        
                        br = rnd.Next(1, 5);
                    }
                    if (br == 1)
                        tref++;
                    if (br == 2)
                        karo++;
                    if (br == 3)
                        pik++;
                    if (br == 4)
                        herc++;

                    matrica[i, j] = br;
                }
            }

            int vrsta = rnd.Next(0, 4);
            int kolona = rnd.Next(0, 4);
            prazno[0] = vrsta;
            prazno[1] = kolona;
            matrica[vrsta, kolona] = -1; 

        }

        private void stampaj()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(matrica[i,j]+" ");
                }
                Console.WriteLine();
            }
        }
        
        public int pomeri(int k,int p)
        {
            int flag = 0;
            if (k == prazno[0])
            {
                if (p==prazno[1]+1)
                {
                    matrica[k, p - 1] = matrica[k,p];
                    matrica[k, p] = -1;
                    prazno[0] = k;
                    prazno[1] = p;
                    flag++;
                }
                else if (p == prazno[1] - 1)
                {
                    matrica[k, p + 1] = matrica[k, p];
                    matrica[k, p] = -1;
                    prazno[0] = k;
                    prazno[1] = p;
                    flag++;
                }
            }
            else if (p == prazno[1])
            {
                if (k==prazno[0]+1)
                {
                    matrica[k - 1, p] = matrica[k, p];
                    matrica[k, p] = -1;
                    prazno[0] = k;
                    prazno[1] = p;
                    flag++;
                }
                else if (k == prazno[0] - 1)
                {
                    matrica[k + 1, p] = matrica[k, p];
                    matrica[k, p] = -1;
                    prazno[0] = k;
                    prazno[1] = p;
                    flag++;
                }
            }
            return flag;
        }
        public bool proveriKraj()
        {
            bool flag1 = true;
            bool flag2 = true;
            for (int i = 0; i < 4; i++)
            {
                int vrednost = matrica[i, 0];
                int vrednost2 = matrica[0, i];
                if (vrednost == -1)
                    vrednost = matrica[i, 1];
                if (vrednost2 == -1)
                    vrednost2 = matrica[1, i];

                for (int j = 0; j < 4; j++)
                {
                    if ((vrednost != matrica[i, j]) && matrica[i, j] != -1)
                        flag1 = false;
                    if ((vrednost2 != matrica[j, i]) && matrica[j, i] != -1)
                        flag2 = false;
                }
            }

            return flag1 || flag2;

        }
        
    }
}
