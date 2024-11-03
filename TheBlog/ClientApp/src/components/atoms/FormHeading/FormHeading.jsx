import './FormHeading.scss';

export default function FormHeading({ children, args }) {
  return <h1 className="userForm__heading" {...args}>{children}</h1>;
}
