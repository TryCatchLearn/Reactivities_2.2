import {useEffect} from 'react';
import {withRouter} from 'react-router-dom';

const ScrollToTop = ({ children, location: { pathname } }: any) => {
    useEffect(() => {
      window.scrollTo(0, 0);
    }, [pathname]);
  
    return children || null;
  };
  
  export default withRouter(ScrollToTop);