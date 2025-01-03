import React, { useEffect, useState } from 'react'
import "./styles.css"
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap'
//@ts-ignore
import userLogo from "../assets/user.svg"
import { Link, useNavigate } from 'react-router-dom'
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useAuth } from '../context/AuthContext'



const validationSchema = Yup.object({
    email: Yup.string()
        .required('Kullanıcı adı zorunludur.'),
    password: Yup.string()
        .required('Parola zorunludur.')
});


export default function DoctorLogin() {

    const { doctorLogin, getAllDoctors } = useAuth()

    const navigate = useNavigate()

    const formData = useFormik({
        initialValues: {
            email: '',
            password: ''
        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            console.log(values)
            // await getAllDoctors()
            await doctorLogin(values)
            navigate("/dashboard")
        }
    });

    return (
        <div className='login-page'>
            <div className='login-container'>
                <div className='login-header'>
                    <h2>Doctor Login</h2>
                    <div className='usr-div'>
                        <img src={userLogo} width={70} height={85} />
                    </div>
                </div>
                <div className='form-container'>
                    <Form onSubmit={formData.handleSubmit}>

                        <FormGroup>
                            <Label className='ms-1'>Email</Label>
                            <Input
                                id='email'
                                name="email"
                                type='text'
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.email}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label className='ms-1'>Password</Label>
                            <Input
                                id="password"
                                name="password"
                                type="password"
                                onChange={formData.handleChange}
                                onBlur={formData.handleBlur}
                                value={formData.values.password}
                            />
                        </FormGroup>

                        {formData.touched.password && formData.errors.password ? (
                            <div style={{ color: "red", textAlign: "center" }}>{formData.errors.password}</div>
                        ) : null}
                        <div className='mt-4' style={{ width: "100%" }}>

                            <Row>
                                <Col>
                                    {/* <Link className="link-component my-4" to="/doctor-login">Doctor Login</Link><br /> */}
                                    <Link className="link-component my-4" to="/doctor-register">Doctor Sing-up</Link>

                                </Col>


                                <Col>
                                    <Link className="link-component my-4" to="/">User Login</Link><br />
                                    <Link className="link-component my-4" to="/user-register">User Sign-up</Link>

                                </Col>
                            </Row>
                        </div>

                        <div className='btn-div'>
                            <Button outline className='login-btn' type='submit'>
                                Login
                            </Button>
                        </div>

                    </Form>
                </div>

            </div >
        </div >
    )
}
