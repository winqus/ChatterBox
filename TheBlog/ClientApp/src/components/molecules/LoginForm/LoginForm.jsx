import './LoginForm.scss';
import { Form } from 'reactstrap';
import FormInputPill from '../../atoms/FormInputPill/FormInputPill';
import FormHeading from '../../atoms/FormHeading/FormHeading';
import FormParagraph from '../../atoms/FormParagraph/FormParagraph';
import { useState } from 'react';
import ActionButton from '../../atoms/ActionButton/ActionButton';
import { useNavigate } from 'react-router-dom';
import { ApplicationPaths } from '../../api-authorization/ApiAuthorizationConstants';
import { Link } from 'react-router-dom';
import ErrorList from '../../atoms/ErrorList/ErrorList';

export default function LoginForm() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState([]);

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const data = new URLSearchParams(new FormData(e.target));
    await fetch(e.target.action, {
      method: "POST",
      body: data,
    })
    .then(async (data) => {
      setIsLoading(false);
      if (data.ok) {
        navigate(ApplicationPaths.Login);
      } else {
        setErrors([`${await data.text()}`]);
      }
    });
  };

  const getFormView = () => (
    <Form className='loginForm' action="/api/account/login" method="post" onSubmit={handleFormSubmit}>
      <FormHeading>Log In</FormHeading>
      <FormParagraph>Provide your user details to access additional site functionality.</FormParagraph>
      <FormInputPill
        id="email_1"
        name="Email"
        label="Email"
        type="email"
        required
      />
      <FormInputPill
        id="password_1"
        name="Password"
        label="Password"
        type="password"
        required
      />
      <ErrorList errors={errors} />
      <FormParagraph>Forgot your <Link to="/account/recoverpassword">password</Link>?</FormParagraph>
      <ActionButton text="Log In" loading={isLoading} args={{type: 'submit'}} />
      <FormParagraph>
        New here? <Link to="/account/register">Create an Account</Link>
      </FormParagraph>
    </Form>
  );

  return getFormView(
    <Form className='loginForm' action="/api/account/login" method="post" onSubmit={handleFormSubmit}>
      <FormHeading>Log In</FormHeading>
      <FormParagraph>Provide your user details to access additional site functionality.</FormParagraph>
      <FormInputPill
        id="email_1"
        name="Email"
        label="Email"
        type="email"
        required
      />
      <FormInputPill
        id="password_1"
        name="Password"
        label="Password"
        type="password"
        required
      />
      <ErrorList errors={errors} />
      <FormParagraph>Forgot your <Link to="/account/recoverpassword">password</Link>?</FormParagraph>
      <ActionButton text="Log In" loading={isLoading} args={{type: 'submit'}} />
      <FormParagraph>
        New here? <Link to="/account/register">Create an Account</Link>
      </FormParagraph>
    </Form>
  );
}
