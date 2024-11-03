import './FormParagraph.scss';

export default function FormParagraph({ children, args }) {
  return <p className="userForm__paragraph" {...args}>{children}</p>;
}
