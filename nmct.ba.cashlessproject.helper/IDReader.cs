using be.belgium.eid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.helper
{
    public class IDReader
    {
        public static BEID_EIDCard getData()
        {
            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderContext Reader = BEID_ReaderSet.instance().getReader();

                if (Reader.isCardPresent())
                {
                    BEID_EIDCard card = Reader.getEIDCard();

                    if (card.isTestCard())
                    {
                        card.setAllowTestCard(true);
                    }

                    return card;
                }
                else
                {
                    return null;
                }
            }

            catch (BEID_Exception)
            {
                BEID_ReaderSet.releaseSDK();
                return null;
            }
        }
    }
}
