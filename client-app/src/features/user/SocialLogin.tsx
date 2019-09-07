import React from 'react';
import { Button, Icon } from 'semantic-ui-react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';

interface IProps {
    responseFacebook: (response: any) => void;
}

const SocialLogin: React.FC<IProps> = ({responseFacebook}) => {
    return (
        <div>
            <FacebookLogin
                appId="895163347527759"
                fields="name, email, picture"
                callback={responseFacebook}
                render={(renderProps: any) => (
                    <Button onClick={renderProps.onClick} type='button' fluid color='facebook'>
                        <Icon name='facebook' />
                        Login with Facebook
                    </Button>
                )}
            />
        </div>
    );
};

export default SocialLogin;