public class main {

    private static int xcounter = 1;
    private static int ycounter = 1;
    private static int x;
    private static int y;

    public static void main(String[] args) {

        //Assuming positive multiples only, as LCM would always be 
        //negative infinity if we included negative multiples

        if(args.length == 2) {

            x = Integer.parseInt(args[0]);
            y = Integer.parseInt(args[1]);

            if(x<0&&y<0) {

                System.out.println("Negative Infinity");

            }else if (x>=0&&y<=0||x<=0&&y>=0){

                System.out.println("0");

            }else {
                LCM(xcounter, ycounter);
            }
        }
    }

    private static void LCM(int xcounter, int ycounter){

        if(x*xcounter<y*ycounter){
            LCM(++xcounter, ycounter);
        }
        else if(x*xcounter>y*ycounter){
            LCM(xcounter, ++ycounter);
        } else {
            //x*xcounter must equal y*ycounter
            System.out.println(x*xcounter);
        }
    }
}
